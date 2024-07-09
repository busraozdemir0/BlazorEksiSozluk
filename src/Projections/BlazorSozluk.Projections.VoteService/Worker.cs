using BlazorSozluk.Common.Events.Entry;
using BlazorSozluk.Common.Infrastructure;
using BlazorSozluk.Common;
using Microsoft.Extensions.Logging;
using BlazorSozluk.Common.Events.EntryComment;

namespace BlazorSozluk.Projections.VoteService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration configuration;
        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;
            this.configuration = configuration;
        }

        // Bir entry'e oy verildiginde oncelikle bu kisim calisiyor yani RabbitMQ'ye kayit islemi gerceklesiyor.
        // Ardindan ilgili VoteService projection'u calistirdigimizda RabbitMQ'daki kayit okunarak DB'ye kaydediliyor
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var connStr = configuration.GetConnectionString("SqlServer");

            var voteService = new Services.VoteService(connStr);

            // Create entry vote
            QueueFactory.CreateBasicConsumer()
            .EnsureExchange(SozlukConstants.VoteExchangeName)
            .EnsureQueue(SozlukConstants.CreateEntryVoteQueueName, SozlukConstants.VoteExchangeName)
            .Receive<CreateEntryVoteEvent>(vote =>
            {
                voteService.CreateEntryVote(vote).GetAwaiter().GetResult();
                _logger.LogInformation("Create Entry Received EntryId: {0}, VoteType: {1}", vote.EntryId, vote.VoteType);
            })
            .StartConsuming(SozlukConstants.CreateEntryVoteQueueName);

            // Delete entry vote
            QueueFactory.CreateBasicConsumer()
            .EnsureExchange(SozlukConstants.VoteExchangeName)
            .EnsureQueue(SozlukConstants.DeleteEntryVoteQueueName, SozlukConstants.VoteExchangeName)
            .Receive<DeleteEntryVoteEvent>(vote =>
            {
              voteService.DeleteEntryVote(vote.EntryId, vote.CreatedBy).GetAwaiter().GetResult();
              _logger.LogInformation("Delete Entry Received EntryId: {0}", vote.EntryId);
            })
            .StartConsuming(SozlukConstants.DeleteEntryVoteQueueName);


            // Create entry comment vote
            QueueFactory.CreateBasicConsumer()
              .EnsureExchange(SozlukConstants.VoteExchangeName)
              .EnsureQueue(SozlukConstants.CreateEntryCommentVoteQueueName, SozlukConstants.VoteExchangeName)
              .Receive<CreateEntryCommentVoteEvent>(vote =>
                {
                   voteService.CreateEntryCommentVote(vote).GetAwaiter().GetResult();
                    _logger.LogInformation("Create Entry Comment Received EntryCommentId: {0}, VoteType: {1}", vote.EntryCommentId, vote.VoteType);
                })
                .StartConsuming(SozlukConstants.CreateEntryCommentVoteQueueName);

            // Delete entry comment vote
            QueueFactory.CreateBasicConsumer()
               .EnsureExchange(SozlukConstants.VoteExchangeName)
               .EnsureQueue(SozlukConstants.DeleteEntryCommentVoteQueueName, SozlukConstants.VoteExchangeName)
               .Receive<DeleteEntryCommentVoteEvent>(vote =>
                {
                   voteService.DeleteEntryCommentVote(vote.EntryCommentId, vote.CreatedBy).GetAwaiter().GetResult();
                   _logger.LogInformation("Delete Entry Comment Received EntryCommentId: {0}", vote.EntryCommentId);
                })
                .StartConsuming(SozlukConstants.DeleteEntryCommentVoteQueueName);
        }
    }
}
