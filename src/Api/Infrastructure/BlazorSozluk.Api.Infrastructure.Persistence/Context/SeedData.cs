using BlazorSozluk.Api.Domain.Models;
using BlazorSozluk.Common.Infrastructure;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSozluk.Api.Infrastructure.Persistence.Context
{
    public class SeedData
    {
        // Bogus kutuphanesi ile User tablosu icin sahte data'lar olusturuyoruz
        private static List<User> GetUsers()
        {
            var result = new Faker<User>("tr")
                .RuleFor(i => i.Id, i => Guid.NewGuid())
                .RuleFor(i => i.CreateDate, i => i.Date.Between(DateTime.Now.AddDays(-100), DateTime.Now)) // Bu tarih araliginda veri gelsin
                .RuleFor(i => i.FirstName, i => i.Person.FirstName)
                .RuleFor(i => i.LastName, i => i.Person.LastName)
                .RuleFor(i => i.EmailAddress, i => i.Internet.Email())
                .RuleFor(i => i.UserName, i => i.Internet.UserName())
                .RuleFor(i => i.Password, i => PasswordEncryptor.Encrpt(i.Internet.Password())) // Sifre hashlenerek tutulmasi icin metod yazdik
                .RuleFor(i => i.EmailConfirmed, i => i.PickRandom(true, false)) // EmailConfirmed alani icin true ya da false arasindan birini rastgele sec
                .Generate(500); // 500 adet User olusturulacak

            return result;
        }

        public async Task SeedAsync(IConfiguration configuration)
        {
            var dbContextBuilder = new DbContextOptionsBuilder();
            dbContextBuilder.UseSqlServer(configuration["BlazorEksiSozlukDbConnectionString"]);

            var context = new BlazorSozlukContext(dbContextBuilder.Options);

            if (context.Users.Any()) // Eger Users tablosunda veri varsa tekrar seed data eklememesi icin
            {
                await Task.CompletedTask;
                return;
            }

            // User Data Seed
            var users = GetUsers();
            var userIds = users.Select(i => i.Id); // Entry ve diger alanlar icin user'larin id'lerini cekiyoruz

            await context.Users.AddRangeAsync(users); // User tablosuna bizim icin 500 tane data ekleyecek

            var guids = Enumerable.Range(0, 150).Select(i => Guid.NewGuid()).ToList();
            int counter = 0;

            // Entry Data Seed
            var entries = new Faker<Entry>("tr")
                .RuleFor(i => i.Id, i => guids[counter++]) // Her seferinde guid'in counter'inci elemanini alsin ve counter'i bir artirsin.
                .RuleFor(i => i.CreateDate, i => i.Date.Between(DateTime.Now.AddDays(-100), DateTime.Now))
                .RuleFor(i => i.Subject, i => i.Lorem.Sentence(5, 5)) // Konu alani icin 5 kelimelik bir cumle olusturacak
                .RuleFor(i => i.Content, i => i.Lorem.Paragraph(2)) // icerik icin 2 adet paragraf olusturacak
                .RuleFor(i => i.CreatedById, i => i.PickRandom(userIds)) // userIds listesine git bunun icerisinden rastgele bir tane sec
                .Generate(150); // Generate metodu ile 150 tane kayit olusturma islemi gerceklestiriyoruz

            await context.Entries.AddRangeAsync(entries);

            // EntryComment Data Seed

            var comments = new Faker<EntryComment>("tr")
                .RuleFor(i => i.Id, i => Guid.NewGuid())
                .RuleFor(i => i.CreateDate, i => i.Date.Between(DateTime.Now.AddDays(-100), DateTime.Now))
                .RuleFor(i => i.Content, i => i.Lorem.Paragraph(2)) // 2 paragraflik content olusturuldu
                .RuleFor(i => i.CreatedById, i => i.PickRandom(userIds))
                .RuleFor(i => i.EntryId, i => i.PickRandom(guids))
                .Generate(1000); // 1000 adet EntryComment olusturulacak

            await context.EntryComments.AddRangeAsync(comments);

            await context.SaveChangesAsync();
        }
    }
}
