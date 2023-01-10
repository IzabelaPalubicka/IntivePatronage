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
                .BeforeMap((src, dest) => dest.BookAuthors?.Clear())
                .ForMember(
                x => x.BookAuthors,
                y => y.MapFrom((src, dest, destMember, context) => src.AuthorsIds.Select(x => new BookAuthor { AuthorId = x, BookId = dest.Id })));
        }
    }
}