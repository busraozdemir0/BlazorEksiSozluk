using BlazorSozluk.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSozluk.Infrastructure.Persistence.Context
{
    public class BlazorSozlukContext : DbContext
    {
        public const string DEFAULT_SCHEMA = "dbo";

        public BlazorSozlukContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Entry> Entries { get; set; }
        public DbSet<EntryVote> EntryVotes { get; set; }
        public DbSet<EntryFavorite> EntryFavorites { get; set; }
        public DbSet<EntryComment> EntryComments { get; set; }
        public DbSet<EntryCommentVote> EntryCommentVotes { get; set; }
        public DbSet<EntryCommentFavorite> EntryCommentFavorites { get; set; }
        public DbSet<EmailConfirmation> EmailConfirmations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public override int SaveChanges()
        {
            OnBeforeSave(); // SaveChanges islemlerinde ilk olarak yazdigimiz bu metod calisacak (CreateDate alani DateTime.Now olarak atanmaktadir)
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSave();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            OnBeforeSave();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            OnBeforeSave();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void OnBeforeSave()
        {
            // DB'ye kayit eklenirken ilgili Entity'i BaseEntity'e donustur.
            var addedEntities = ChangeTracker.Entries()
                                .Where(i => i.State == EntityState.Added)
                                .Select(i => (BaseEntity)i.Entity);

            // Eklenecek ilgili kaydin bu metodda CreateDate'inin su anki tarih saat olarak kaydolmasini saglar.
            PrepareAddedEntities(addedEntities);
        }

        private void PrepareAddedEntities(IEnumerable<BaseEntity> entities)
        {
            // Gelen entity'ler arasinda donerek CreatedDate alanini o anki tarih zaman olarak kaydolmasini saglayacak.
            foreach (var entity in entities)
            {
                if (entity.CreateDate == DateTime.MinValue)
                    entity.CreateDate = DateTime.Now;
            }
        }
    }
}
