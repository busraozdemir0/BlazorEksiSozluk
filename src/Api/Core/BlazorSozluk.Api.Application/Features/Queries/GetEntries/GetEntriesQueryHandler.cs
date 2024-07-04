using AutoMapper;
using AutoMapper.QueryableExtensions;
using BlazorSozluk.Api.Application.Features.Queries.GetEntries;
using BlazorSozluk.Api.Application.Interfaces.Repositories;
using BlazorSozluk.Common.Models.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetEntriesQueryHandler : IRequestHandler<GetEntriesQuery, List<GetEntriesViewModel>>
{
    private readonly IEntryRepository entryRepository;
    private readonly IMapper mapper;

    public GetEntriesQueryHandler(IEntryRepository entryRepository, IMapper mapper)
    {
        this.entryRepository = entryRepository;
        this.mapper = mapper;
    }

    public async Task<List<GetEntriesViewModel>> Handle(GetEntriesQuery request, CancellationToken cancellationToken)
    {
        var query = entryRepository.AsQueryable();

        if (request.TodayEntries)
        {
            query = query.Where(i => i.CreateDate >= DateTime.Now.Date) // Bugunun ilk saatlerini alacak (gece saat 00.00 iken)
                         .Where(i => i.CreateDate <= DateTime.Now.AddDays(1).Date); // Burada da bugune 1 gun ekleyip bir sonraki gun saat 00.00!a kadar sayilmis olacak (Boylelikle 24 saati iceren bir gun tanimlamis olduk)
        }

        // EntryComment icin Butun kayitlari rastgele siraladiktan sonra 100 tane(Count degiskeni kadar) kayit getirecek
        query = query.Include(i => i.EntryComments)
            .OrderBy(i => Guid.NewGuid()) // rastgele siralama olusmasi icin
            .Take(request.Count);

        return await query.ProjectTo<GetEntriesViewModel> (mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
