using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSozluk.Api.Application.Features.Commands.EntryComment.CreateFav
{
    public class CreateEntryCommentFavCommand:IRequest<bool>
    {
        public CreateEntryCommentFavCommand(Guid entryCommentId, Guid userId)
        {
            EntryCommentId = entryCommentId;
            UserId = userId;
        }

        public Guid EntryCommentId { get; set; } // EntryCommentId bilgisi
        public Guid UserId { get; set; } // UserId hangi kullanici kendi favorilerine eklemek istiyor
    }
}
