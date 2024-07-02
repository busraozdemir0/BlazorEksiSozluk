using BlazorSozluk.Api.Application.Interfaces.Repositories;
using BlazorSozluk.Api.Domain.Models;
using BlazorSozluk.Api.Infrastructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSozluk.Api.Infrastructure.Persistence.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository //  IUserRepository icerisinde genericten haric baska bir metod varsa diye miraz aliyoruz.
    {
        public UserRepository(BlazorSozlukContext dbContext) : base(dbContext)
        {
        }

    }
}
