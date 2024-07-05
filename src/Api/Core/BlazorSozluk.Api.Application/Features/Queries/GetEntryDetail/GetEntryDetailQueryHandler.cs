using BlazorSozluk.Api.Application.Interfaces.Repositories;
using BlazorSozluk.Common.Infrastructure.Extensions;
using BlazorSozluk.Common.Models.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSozluk.Api.Application.Features.Queries.GetEntryDetail
{
    public class GetEntryDetailQueryHandler : IRequestHandler<GetEntryDetailQuery, GetEntryDetailViewModel>
    {
        private readonly IEntryRepository entryRepository;

        public GetEntryDetailQueryHandler(IEntryRepository entryRepository)
        {
            this.entryRepository = entryRepository;
        }

        public async Task<GetEntryDetailViewModel> Handle(GetEntryDetailQuery request, CancellationToken cancellationToken)
        {
            var query = entryRepository.AsQueryable();

            query = query.Include(i => i.EntryFavorites)
                        .Include(i => i.CreatedBy)
                        .Include(i => i.EntryVotes)
                        .Where(i => i.Id == request.EntryId);

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
                    : Common.Models.VoteType.None // Disaridan UserId gelmiyorsa veya EntryVote tablosunda o kullanicinin ilgili Entry icin Vote'u yoksa VoteType'ini None olarak set ettik
            });

            return await list.FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }
    }
}
