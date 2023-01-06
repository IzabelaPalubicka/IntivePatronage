using Microsoft.EntityFrameworkCore;
using Patronage.Application.Filters;
using Patronage.Database;
using Patronage.Database.Entities;

namespace Patronage.Application.Repositories
{
    public class BookService : IBookService
    {
        private readonly ApplicationDBContext _context;
        public BookService(ApplicationDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> AddBookAsync(Book book)
        {
            if (book is null)
            {
                throw new ArgumentNullException(nameof(book));
            }

            try
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    _context.Books.Add(book);

                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> DeleteBookAsync(Book book)
        {
            try
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    _context.Books.Remove(book);

                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public async Task<IEnumerable<Book>> GetFilteredBookAsync(BookFilter filter)
        {
            return await _context.Books.Where(x =>
                x.Title.ToLower().Contains(filter.Title.ToLower())
            &&
            x.ISBN.ToLower().Equals(filter.ISBN.ToLower())
            &&
            x.Rating.CompareTo(filter.Rating) >= 0 &&
            x.PublicationDate.CompareTo(filter.PublicationDateStartPeriod) >= 0 &&
            x.PublicationDate.CompareTo(filter.PublicationDateEndPeriod) <= 0
            ).ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksAsync()
        {
            return await _context.Books.ToListAsync();
        }
        public Task<bool> UpdateBookAsync(Book book)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }

        public async Task<Book> GetBookAsync(int bookId)
        {
            return await _context.Books.Where(c => c.Id == bookId).FirstOrDefaultAsync();
        }

        public async Task<bool> AuthorExistAsync(int authorId)
        {
            return await _context.Authors.AnyAsync(a => a.Id == authorId);
        }
    }
}
