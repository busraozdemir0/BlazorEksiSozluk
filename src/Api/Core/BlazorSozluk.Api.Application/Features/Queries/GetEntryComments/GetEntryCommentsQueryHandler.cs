using AutoMapper;
using BlazorSozluk.Api.Application.Interfaces.Repositories;
using BlazorSozluk.Common.Infrastructure.Extensions;
using BlazorSozluk.Common.Models.Page;
using BlazorSozluk.Common.Models.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSozluk.Api.Application.Features.Queries.GetEntryComments
{
    public class GetEntryCommentsQueryHandler : IRequestHandler<GetEntryCommentsQuery, PagedViewModel<GetEntryCommentsViewModel>>
    {
        private readonly IEntryCommentRepository entryCommentRepository;

        public GetEntryCommentsQueryHandler(IEntryCommentRepository entryCommentRepository)
        {
            this.entryCommentRepository = entryCommentRepository;
        }

        public async Task<PagedViewModel<GetEntryCommentsViewModel>> Handle(GetEntryCommentsQuery request, CancellationToken cancellationToken)
        {
            var query = entryCommentRepository.AsQueryable();

            query = query.Include(i => i.EntryCommentFavorites)
                        .Include(i => i.CreatedBy)
                        .Include(i => i.EntryCommentVotes)
                        .Where(i => i.EntryId == request.EntryId);

            var list = query.Select(i => new GetEntryCommentsViewModel()
            {
                Id = i.Id,
                Content = i.Content,
                IsFavorited = request.UserId.HasValue && i.EntryCommentFavorites.Any(j => j.CreatedById == request.UserId), // Cagirdigimiz entry icin favoriler tablosuna bak. Disaridan gelen UserId bilgisi tarafindan favorilere eklenmis mi
                FavoritedCount = i.EntryCommentFavorites.Count,
                CreatedDate = i.CreateDate,
                CreatedByUserName = i.CreatedBy.UserName,
                // Disaridan gonderilen UserId var ise ve bu user tarafindan Vote eklenmis ise
                VoteType =
                    request.UserId.HasValue && i.EntryCommentVotes.Any(j => j.CreatedById == request.UserId)
                    ? i.EntryCommentVotes.FirstOrDefault(j => j.CreatedById == request.UserId).VoteType
                    : BlazorSozluk.Common.Models.VoteType.None // Disaridan UserId gelmiyorsa veya EntryVote tablosunda o kullanicinin ilgili Entry icin Vote'u yoksa VoteType'ini None olarak set ettik
            });

            var entries = await list.GetPaged(request.Page, request.PageSize);

            return entries;
        }
    }
}
