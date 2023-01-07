using Patronage.Application.Filters;
using Patronage.Application.Models.Book;
using Patronage.Database.Entities;

namespace Patronage.Application.Repositories
{
    public interface IBookService
    {
        Task<BookDto> AddBookAsync(CreateBookDto book);

        Task<IEnumerable<BookDto>> GetBooksAsync();

        Task UpdateBookAsync(UpdateBookDto book);

        Task DeleteBookAsync(int id);

        Task<IEnumerable<BookDto>> GetFilteredBookAsync(BookFilter filter);

        Task<Book?> GetBookAsync(int id);
    }
}