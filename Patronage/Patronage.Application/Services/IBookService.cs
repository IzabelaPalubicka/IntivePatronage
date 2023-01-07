using Patronage.Application.Filters;
using Patronage.Application.Models.Book;
using Patronage.Database.Entities;

namespace Patronage.Application.Repositories
{
    public interface IBookService
    {
        Task<bool> AddBookAsync(Book book);
        Task<IEnumerable<Book>> GetBooksAsync();
        Task<bool> UpdateBookAsync(int id, UpdateBookDto book);
        Task<bool> DeleteBookAsync(Book book);
        Task<IEnumerable<Book>> GetFilteredBookAsync(BookFilter filter);
        Task<Book> GetBookAsync(int bookId);
    }
}
