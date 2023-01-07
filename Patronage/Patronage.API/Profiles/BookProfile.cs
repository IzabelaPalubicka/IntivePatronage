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
            CreateMap<CreateBookDto, Book>()
                .ForMember(
                x => x.BookAuthors,
                y => y.MapFrom((src, dest, destMember, context) => src.AuthorsIds.Select(x => new BookAuthor { AuthorId = x })));
            CreateMap<UpdateBookDto, Book>()
                .ForMember(
                x => x.BookAuthors,
                y => y.MapFrom((src, dest, destMember, context) => src.AuthorsIds.Select(x => new BookAuthor { AuthorId = x })));
            CreateMap<Book, UpdateBookDto>();
        }
    }
}