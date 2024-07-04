using BlazorSozluk.Common.Models.Page;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BlazorSozluk.Common.Infrastructure.Extensions
{
    public static class PagingExtensions
    {
        // Sayfalama mekanizmasi icin kullanilacak metod
        public static async Task<PagedViewModel<T>> GetPaged<T>(this IQueryable<T> query,
                                                                int currentPage,
                                                                int pageSize) where T: class
        {
            var count = await query.CountAsync();

            Page paging = new(currentPage, pageSize, count);

            var data = await query.Skip(paging.Skip).Take(paging.PageSize).AsNoTracking().ToListAsync(); // Ornegin ilk 20 kaydi atla(skip) ondan sonraki 10 kaydi getir

            var result = new PagedViewModel<T>(data, paging);

            return result;
        }     
    }
}
