using AutoMapper;
using BlazorSozluk.Api.Domain.Models;
using BlazorSozluk.Common.Models.Queries;
using BlazorSozluk.Common.Models.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSozluk.Api.Application.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<User, LoginUserViewModel>()
                .ReverseMap();

            CreateMap<User, CreateUserCommand>()
               .ReverseMap();

            CreateMap<User, UpdateUserCommand>()
             .ReverseMap();
              
            CreateMap<Entry, CreateEntryCommand>()
             .ReverseMap();

            CreateMap<EntryComment, CreateEntryCommentCommand>()
             .ReverseMap();

            // Bu ViewModel'de CommentCount alani Entry'de olmadigi icin bu alani otomatik set edemeyeceginden EntryComments altindaki Count bilgisini set etmesi gerektigini soyledik.
            CreateMap<Entry, GetEntriesViewModel>()
                .ForMember(x => x.CommentCount, y => y.MapFrom(z => z.EntryComments.Count));

        }
    }
}
