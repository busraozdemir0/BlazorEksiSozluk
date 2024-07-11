using BlazorSozluk.Common.Infrastructure;
using BlazorSozluk.Common;
using BlazorSozluk.Common.Events.User;
using BlazorSozluk.Projections.UserService.Services;

namespace BlazorSozluk.Projections.UserService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly Services.UserService userService;
        private readonly EmailService emailService;
        public Worker(ILogger<Worker> logger, Services.UserService userService, EmailService emailService)
        {
            _logger = logger;
            this.userService = userService;
            this.emailService = emailService;
        }
        // RABBÝTMQ MANAGEMENT'E ULASMAK ÝCÝN DEFAULT OLAN 15672 PORTUNDAN ULASÝRÝZ => http://localhost:15672/
        // Projections islemleri RabbitMQ vasitasiyla calistirilmaktadir.
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // ** Burasi calistigi zaman RabbitMQ'daki kuyruga (UserEmailChangedEvent kuyruguna) kendisini kayit edecek.
            // Daha sonra bu kuyruga bir kayit geldiginde once EmailConfirmation tablosuna bir kayit atacak, ardindan bir link olusturacak ve o linki ekrana bastiracak.
            // Bu linki browser uzerinden cagirdigimiz zaman bizim EmailAdresimiz onaylanmis olacak.

            QueueFactory.CreateBasicConsumer()
                 .EnsureExchange(SozlukConstants.UserExchangeName)
                 .EnsureQueue(SozlukConstants.UserEmailChangedQueueName, SozlukConstants.UserExchangeName)
                 .Receive<UserEmailChangedEvent>(user =>
                 { // *** Email degistirildiginde EmailConfirmation alani otomatikman false oluyor. Email onaylama asamasi icin
                   //+ RabbitMQ yardimiyla bir kuyruk olusturup link olusturulmasini sagliyoruz. Bu linki swagger'da post ettigimizde ise EmailConfirmation alani true olacaktir.

                     // DB insert
                     var confirmationId = userService.CreateEmailConfirmation(user).GetAwaiter().GetResult();

                     // Generate link
                     var link = emailService.GenerateConfirmationLink(confirmationId);

                     //Send Email (Biz email gondermek yerine linki Console ekranina bastirmaktayiz.)

                     emailService.SendEMail(user.NewEmailAddress, link).GetAwaiter().GetResult();

                 })
                 .StartConsuming(SozlukConstants.UserEmailChangedQueueName);
        
        }
    }
}
