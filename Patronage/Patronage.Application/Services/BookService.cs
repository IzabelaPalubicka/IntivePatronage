using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Patronage.Application.Filters;
using Patronage.Application.Models.Book;
using Patronage.Database;
using Patronage.Database.Entities;
using System.Linq.Expressions;

namespace Patronage.Application.Repositories
{
    public class BookService : IBookService
    {
        private readonly ApplicationDBContext _context;
        private readonly IMapper _mapper;

        public BookService(ApplicationDBContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
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
            return await _context.Books
                .Where(HasTitle(filter))
                .Where(HasISBN(filter))
                .Where(HasRaiting(filter))
                .Where(HasPublicationDateFrom(filter))
                .Where(HasPublicationDateTo(filter))
                .ToListAsync();
        }

        static Expression<Func<Book, bool>> HasTitle(BookFilter filter) => book => (filter.Title != null && book.Title.ToLower().Contains(filter.Title.ToLower())) || filter.Title == null;
        static Expression<Func<Book, bool>> HasISBN(BookFilter filter) => book => (filter.ISBN != null && book.ISBN.Contains(filter.ISBN)) || filter.ISBN == null;
        static Expression<Func<Book, bool>> HasRaiting(BookFilter filter) => book => (filter.Rating != null && book.Rating >= filter.Rating) || filter.Rating == null;
        static Expression<Func<Book, bool>> HasPublicationDateFrom(BookFilter filter) => book => (filter.PublicationDateStartPeriod != null && book.PublicationDate >= filter.PublicationDateStartPeriod) || filter.PublicationDateStartPeriod == null;
        static Expression<Func<Book, bool>> HasPublicationDateTo(BookFilter filter) => book => (filter.PublicationDateEndPeriod != null && book.PublicationDate <= filter.PublicationDateEndPeriod) || filter.PublicationDateEndPeriod == null;

        public async Task<IEnumerable<Book>> GetBooksAsync()
        {
            return await _context.Books.ToListAsync();
        }
        public async Task<bool> UpdateBookAsync(int id, UpdateBookDto updateBookDto)
        {
            try
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    var bookEntity = await GetBookAsync(id);

                    if (bookEntity == null)
                    {
                        return false;
                    }

                    _mapper.Map(updateBookDto, bookEntity);

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

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }

        public async Task<Book> GetBookAsync(int bookId)
        {
            return await _context.Books.Where(c => c.Id == bookId).Include(x => x.BookAuthors).FirstOrDefaultAsync();
        }

        //public async Task<bool> AuthorExistAsync(int authorId)
        //{
        //    return await _context.Authors.AnyAsync(a => a.Id == authorId);
        //}
    }
}
