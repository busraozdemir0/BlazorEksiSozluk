using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BlazorSozluk.Common.Models.Queries;
using MediatR;

namespace BlazorSozluk.Common.Models.RequestModels
{
    public class LoginUserCommand : IRequest<LoginUserViewModel>
    {
        public LoginUserCommand(string emailAddress, string password)
        {
            EmailAddress = emailAddress;
            Password = password;
        }
        public LoginUserCommand()
        {
            
        }

        public string EmailAddress { get; set; } // Sadece iceriden set edilsinler
        public string Password { get; set; }


    }
}
