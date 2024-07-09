using BlazorSozluk.Api.Application.Interfaces.Repositories;
using BlazorSozluk.Common.Infrastructure.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSozluk.Api.Application.Features.Commands.User.ConfirmEmail
{
    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, bool>
    {
        private readonly IUserRepository userRepository;
        private readonly IEmailConfirmationRepository emailConfirmationRepository;

        public ConfirmEmailCommandHandler(IUserRepository userRepository, IEmailConfirmationRepository emailConfirmationRepository)
        {
            this.userRepository = userRepository;
            this.emailConfirmationRepository = emailConfirmationRepository;
        }

        public async Task<bool> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var confirmation = await emailConfirmationRepository.GetByIdAsync(request.ConfirmationId);

            if (confirmation is null)
                throw new DatabaseValidationException("Onay bulunamadı!");

            var dbUser = await userRepository.GetSingleAsync(i => i.EmailAddress == confirmation.NewEmailAddress);

            if (dbUser is null)
                throw new DatabaseValidationException("Bu e-postayla kullanıcı bulunamadı!"); // bu emaile ait kullanici bulunamadi

            if (dbUser.EmailConfirmed)
                throw new DatabaseValidationException("Bu Email adresi zaten onaylanmış!"); // EmailConfirmation alani true ise bu emaili zaten dogrulanmis hatasi dondurur.

            dbUser.EmailConfirmed = true;
            await userRepository.UpdateAsync(dbUser);

            return true;
        }
    }
}
