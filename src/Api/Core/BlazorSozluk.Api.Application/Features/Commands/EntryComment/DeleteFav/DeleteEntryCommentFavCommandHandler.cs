using BlazorSozluk.Common.Events.Entry;
using BlazorSozluk.Common.Infrastructure;
using BlazorSozluk.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorSozluk.Common.Events.EntryComment;

namespace BlazorSozluk.Api.Application.Features.Commands.EntryComment.DeleteFav
{
    public class DeleteEntryCommentFavCommandHandler : IRequestHandler<DeleteEntryCommentFavCommand, bool>
    {
        public async Task<bool> Handle(DeleteEntryCommentFavCommand request, CancellationToken cancellationToken)
        {
            QueueFactory.SendMessageToExchange(exchangeName: SozlukConstants.FavExchangeName,
               exchangeType: SozlukConstants.DefaultExchangeType,
               queueName: SozlukConstants.DeleteEntryCommentFavQueueName,
               obj: new DeleteEntryCommentFavEvent()
               {
                   EntryCommentId = request.EntryCommentId,
                   CreatedBy = request.UserId,
               });

            return await Task.FromResult(true);
        }
    }
}
