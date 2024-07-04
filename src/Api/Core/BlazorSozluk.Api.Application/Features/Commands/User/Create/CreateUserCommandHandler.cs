using AutoMapper;
using BlazorSozluk.Api.Application.Interfaces.Repositories;
using BlazorSozluk.Common;
using BlazorSozluk.Common.Events.User;
using BlazorSozluk.Common.Infrastructure;
using BlazorSozluk.Common.Infrastructure.Exceptions;
using BlazorSozluk.Common.Models.RequestModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSozluk.Api.Application.Features.Commands.User.Create
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public CreateUserCommandHandler(IMapper mapper, IUserRepository userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var existsUser = await _userRepository.GetSingleAsync(i => i.EmailAddress == request.EmailAddress); // Girilen e-mail'e ait onceden db'ye user kaydolmus mu?


            if (existsUser is not null)
                throw new DatabaseValidationException("User already exists!");

            var dbUser = _mapper.Map<Domain.Models.User>(request);

            // rows degiskeni kayitin basariyla insert edilip edilmedigi bilgisini donecek
            var rows = await _userRepository.AddAsync(dbUser);

            // Email Changed/Created (Email yeni olusturuldugu icin email confirmation islemi kullaniciya yaptirilacak)

            if (rows > 0) // rows degiskeni kayit edilip edilmedigi bilgisini int olarak tutar ve eger 0'dan buyuksa kayit edilmistir anlaminda
            {
                var @event = new UserEmailChangedEvent()
                {
                    OldEmailAddress = null,
                    NewEmailAddress = dbUser.EmailAddress
                };
                QueueFactory.SendMessageToExchange(exchangeName: SozlukConstants.UserExchangeName,
                                                    exchangeType: SozlukConstants.DefaultExchangeType,
                                                    queueName: SozlukConstants.UserEmailChangedQueueName,
                                                    obj: @event);
            }

            return dbUser.Id;
        }
    }
}
