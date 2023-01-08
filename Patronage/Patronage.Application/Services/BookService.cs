using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Patronage.Application.Exceptions;
using Patronage.Application.Filters;
using Patronage.Application.Models.Book;
using Patronage.Database;
using Patronage.Database.Entities;
using System.Data;
using System.Linq.Expressions;

namespace Patronage.Application.Repositories
{
    public class BookService : IBookService
    {
        private readonly ApplicationDBContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<BookService> _logger;
        private readonly IConfigurationProvider _configuration;

        public BookService(ApplicationDBContext context, IMapper mapper, ILogger<BookService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger;
            _configuration = _mapper.ConfigurationProvider;
        }

        // <inheritdoc />
        public async Task<BookDto> AddBookAsync(CreateBookDto createBookDto)
        {
            var bookEntity = _mapper.Map<Book>(createBookDto);

            using var transaction = _context.Database.BeginTransaction();

            try
            {
                _context.Books.Add(bookEntity);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                transaction.Rollback();
                _logger.LogError(e.Message);
                throw;
            }

            return _mapper.Map<BookDto>(bookEntity);
        }

        // <inheritdoc />
        public async Task DeleteBookAsync(int id)
        {
            var book = await GetBookAsync(id);

            if (book is null)
            {
                throw new NotFoundException($"Book with {id} not found");
            }

            using var transaction = _context.Database.BeginTransaction();

            try
            {
                _context.Books.Remove(book);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                transaction.Rollback();
                _logger.LogError(e.Message);
                throw;
            }
        }

        // <inheritdoc />
        public async Task<IEnumerable<BookDto>> GetFilteredBooksAsync(BookFilter filter)
        {
            return await _context.Books
                .Where(HasTitle(filter))
                .Where(HasISBN(filter))
                .Where(HasRaiting(filter))
                .Where(HasPublicationDateFrom(filter))
                .Where(HasPublicationDateTo(filter))
                .ProjectTo<BookDto>(_configuration)
                .ToListAsync();
        }

        // <inheritdoc />
        public async Task<IEnumerable<BookDto>> GetBooksAsync()
        {
            return await _context.Books
                .ProjectTo<BookDto>(_configuration)
                .ToListAsync();
        }

        // <inheritdoc />
        public async Task UpdateBookAsync(UpdateBookDto updateBookDto)
        {
            var bookEntity = await GetBookAsync(updateBookDto.Id);

            if (bookEntity is null)
            {
                throw new NotFoundException($"Book with {updateBookDto.Id} not found");
            }

            using var transaction = _context.Database.BeginTransaction();

            try
            {
                _mapper.Map(updateBookDto, bookEntity);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                transaction.Rollback();
                _logger.LogError(e.Message);
                throw;
            }
        }

        // <inheritdoc />
        public async Task<Book?> GetBookAsync(int id)
        {
            return await _context.Books
                .Include(x => x.BookAuthors)
                .Where(x => x.Id == id)
                .Select(x => new Book
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    Rating = x.Rating,
                    ISBN = x.ISBN,
                    PublicationDate = x.PublicationDate,
                })
                .FirstOrDefaultAsync();
        }

        private static Expression<Func<Book, bool>> HasTitle(BookFilter filter) => book => (filter.Title != null && book.Title.ToLower().Contains(filter.Title.ToLower())) || filter.Title == null;

        private static Expression<Func<Book, bool>> HasISBN(BookFilter filter) => book => (filter.ISBN != null && book.ISBN.Contains(filter.ISBN)) || filter.ISBN == null;

        private static Expression<Func<Book, bool>> HasRaiting(BookFilter filter) => book => (filter.Rating != null && book.Rating >= filter.Rating) || filter.Rating == null;

        private static Expression<Func<Book, bool>> HasPublicationDateFrom(BookFilter filter) => book => (filter.PublicationDateStartPeriod != null && book.PublicationDate >= filter.PublicationDateStartPeriod) || filter.PublicationDateStartPeriod == null;

        private static Expression<Func<Book, bool>> HasPublicationDateTo(BookFilter filter) => book => (filter.PublicationDateEndPeriod != null && book.PublicationDate <= filter.PublicationDateEndPeriod) || filter.PublicationDateEndPeriod == null;
    }
}