using BlazorSozluk.Common;
using BlazorSozluk.Common.Events.Entry;
using BlazorSozluk.Common.Events.EntryComment;
using BlazorSozluk.Common.Infrastructure;

namespace BlazorSozluk.Projections.FavoriteService
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
        // RabbitMQ'yu cihazýmýzda start edip(RabbitMQ Service - start) varsayilan olarak http://localhost:15672/ bu yoldan RabbitMQ paneline ulasabiliriz.

        // Bir Entry'i favorilere ekledigimizde once RabbitMQ'ye kaydolacak ardindan yazdigimiz bu WorkerService ile db'ye kaydedilecek.
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var connStr = configuration.GetConnectionString("SqlServer");
            
            var favService = new Services.FavoriteService(connStr);

            // 4 AYRÝ KUyRUK ÝSLEMÝ VAR. BU 4 KUYRUGU RabbitMQ DÝNLEYECEK

            // RabbitMQ'ya entry create islemi
            QueueFactory.CreateBasicConsumer()
                .EnsureExchange(SozlukConstants.FavExchangeName)
                .EnsureQueue(SozlukConstants.CreateEntryFavQueueName, SozlukConstants.FavExchangeName)
                .Receive<CreateEntryFavEvent>(async fav =>
                { // queue'ye bir sey geldiginde ne yapilacak

                    // db insert
                     favService.CreateEntryFav(fav).GetAwaiter().GetResult();
                    _logger.LogInformation($"Received EntryId {fav.EntryId}");
                })
                .StartConsuming(SozlukConstants.CreateEntryFavQueueName);


            // RabbitMQ'ya entry delete islemi
            QueueFactory.CreateBasicConsumer()
                .EnsureExchange(SozlukConstants.FavExchangeName)
                .EnsureQueue(SozlukConstants.DeleteEntryFavQueueName, SozlukConstants.FavExchangeName)
                .Receive<DeleteEntryFavEvent>(async fav =>
                { // queue'ye bir sey geldiginde ne yapilacak

                    // db delete
                    favService.DeleteEntryFav(fav).GetAwaiter().GetResult();
                    _logger.LogInformation($"Deleted Received EntryId {fav.EntryId}");
                })
                .StartConsuming(SozlukConstants.DeleteEntryFavQueueName);


            // RabbitMQ'ya entry comment create  islemi
            QueueFactory.CreateBasicConsumer()
                .EnsureExchange(SozlukConstants.FavExchangeName)
                .EnsureQueue(SozlukConstants.CreateEntryCommentFavQueueName, SozlukConstants.FavExchangeName)
                .Receive<CreateEntryCommentFavEvent>(async fav =>
                { // queue'ye bir sey geldiginde ne yapilacak

                    // db insert
                    favService.CreateEntryCommentFav(fav).GetAwaiter().GetResult();
                    _logger.LogInformation($"Create EntryComment Received EntryCommentId {fav.EntryCommentId}");
                })
                .StartConsuming(SozlukConstants.CreateEntryCommentFavQueueName);


            // RabbitMQ'ya entry comment delete islemi
            QueueFactory.CreateBasicConsumer()
                .EnsureExchange(SozlukConstants.FavExchangeName)
                .EnsureQueue(SozlukConstants.DeleteEntryCommentFavQueueName, SozlukConstants.FavExchangeName)
                .Receive<DeleteEntryCommentFavEvent>(async fav =>
                { // queue'ye bir sey geldiginde ne yapilacak

                    // db delete
                    favService.DeleteEntryCommentFav(fav).GetAwaiter().GetResult();
                    _logger.LogInformation($"Deleted Received EntryCommentId {fav.EntryCommentId}");
                })
                .StartConsuming(SozlukConstants.DeleteEntryCommentFavQueueName);
        }
    }
}
