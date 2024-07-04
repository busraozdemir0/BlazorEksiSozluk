using BlazorSozluk.Common.Models.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSozluk.Api.Application.Features.Queries.GetEntries
{
    public class GetEntriesQuery : IRequest<List<GetEntriesViewModel>>
    {
        public bool TodayEntries { get; set; } // Bugune ait kayitlari dondurmek icin
        public int Count { get; set; } = 100; // Varsayilan olarak 100 kayit listelenecek
    }
}
