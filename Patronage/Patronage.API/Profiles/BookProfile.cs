using AutoMapper;
using Patronage.Application.Models.Book;
using Patronage.Database.Entities;

namespace Patronage.API.Profiles
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Book, BookDto>();
            CreateMap<CreateBookDto, Book>();
            CreateMap<UpdateBookDto, Book>();
            CreateMap<Book, UpdateBookDto>();
        }
    }
}
