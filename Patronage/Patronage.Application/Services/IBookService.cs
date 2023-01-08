using Patronage.Application.Filters;
using Patronage.Application.Models.Book;
using Patronage.Database.Entities;

namespace Patronage.Application.Repositories
{
    /// <summary>
    /// Service handling book operations
    /// </summary>
    public interface IBookService
    {
        /// <summary>
        /// Add a book
        /// </summary>
        /// <param name="createBookDto">Book to add</param>
        /// <returns>The dto of added book</returns>
        Task<BookDto> AddBookAsync(CreateBookDto book);

        /// <summary>
        /// Get all books
        /// </summary>
        /// <returns>The dtos of received books</returns>
        Task<IEnumerable<BookDto>> GetBooksAsync();

        /// <summary>
        /// Update a book
        /// </summary>
        /// <param name="updateBookDto">A book to update</param>
        /// <returns></returns>
        /// <exception cref="NotFoundException">Exception that is triggered when a book with the id does not exist</exception>
        Task UpdateBookAsync(UpdateBookDto book);

        /// <summary>
        /// Delete a book by id
        /// </summary>
        /// <param name="id">The id of the book to delete</param>
        /// <returns></returns>
        /// <exception cref="NotFoundException">Exception that is triggered when a book with the id does not exist</exception>
        Task DeleteBookAsync(int id);

        /// <summary>
        /// Get filtered books by filtered parameters
        /// </summary>
        /// <param name="filter">The filter parameters</param>
        /// <returns>The dtos of the filtered books</returns>
        Task<IEnumerable<BookDto>> GetFilteredBooksAsync(BookFilter filter);

        /// <summary>
        /// Get book by id
        /// </summary>
        /// <param name="id">The book id</param>
        /// <returns>The dto of received book</returns>
        Task<Book?> GetBookAsync(int id);
    }
}