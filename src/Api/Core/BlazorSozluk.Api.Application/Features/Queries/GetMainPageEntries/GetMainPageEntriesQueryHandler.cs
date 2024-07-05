using BlazorSozluk.Api.Application.Features.Queries.GetMainPageEntries;
using BlazorSozluk.Api.Application.Interfaces.Repositories;
using BlazorSozluk.Common.Infrastructure.Extensions;
using BlazorSozluk.Common.Models.Page;
using BlazorSozluk.Common.Models.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetMainPageEntriesQueryHandler : IRequestHandler<GetMainPageEntriesQuery, PagedViewModel<GetEntryDetailViewModel>>
{
    private readonly IEntryRepository entryRepository;
    public GetMainPageEntriesQueryHandler(IEntryRepository entryRepository)
    {
        this.entryRepository = entryRepository;
    }
    public async Task<PagedViewModel<GetEntryDetailViewModel>> Handle(GetMainPageEntriesQuery request, CancellationToken cancellationToken)
    {
        var query = entryRepository.AsQueryable();

        query = query.Include(i => i.EntryFavorites)
                    .Include(i => i.CreatedBy)
                    .Include(i => i.EntryVotes);

        var list = query.Select(i => new GetEntryDetailViewModel()
        {
            Id = i.Id,
            Subject = i.Subject,
            Content = i.Content,
            IsFavorited = request.UserId.HasValue && i.EntryFavorites.Any(j => j.CreatedById == request.UserId), // Cagirdigimiz entry icin favoriler tablosuna bak. Disaridan gelen UserId bilgisi tarafindan favorilere eklenmis mi
            FavoritedCount = i.EntryFavorites.Count,
            CreatedDate = i.CreateDate,
            CreatedByUserName = i.CreatedBy.UserName,
            // Disaridan gonderilen UserId var ise ve bu user tarafindan Vote eklenmis ise
            VoteType =
                request.UserId.HasValue && i.EntryVotes.Any(j => j.CreatedById == request.UserId)
                ? i.EntryVotes.FirstOrDefault(j => j.CreatedById == request.UserId).VoteType
                : BlazorSozluk.Common.Models.VoteType.None // Disaridan UserId gelmiyorsa veya EntryVote tablosunda o kullanicinin ilgili Entry icin Vote'u yoksa VoteType'ini None olarak set ettik
        });

        var entries = await list.GetPaged(request.Page, request.PageSize);

        return entries;
    }
}
