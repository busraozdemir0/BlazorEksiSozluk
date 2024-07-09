using BlazorSozluk.Common.Models.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSozluk.Api.Application.Features.Queries.GetUserDetail
{
    public class GetUserDetailQuery : IRequest<UserDetailViewModel>
    {
        public GetUserDetailQuery(Guid userId, string userName = null)
        {
            UserId = userId;
            UserName = userName;
        }

        public Guid UserId { get; set; }
        public string UserName { get; set; }
    }
}
