using AutoMapper;
using Patronage.Application.Models.Author;
using Patronage.Database.Entities;

namespace Patronage.API.Profiles
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<Author, AuthorDto>();
            CreateMap<CreateAuthorDto, Author>();
        }
    }
}
