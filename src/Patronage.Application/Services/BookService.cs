using System.Data;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Patronage.Application.Exceptions;
using Patronage.Application.Filters;
using Patronage.Application.Models.Book;
using Patronage.Database;
using Patronage.Database.Entities;

namespace Patronage.Application.Services
{
    public class BookService : IBookService
    {
        private readonly ApplicationDBContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<BookService> _logger;
        private readonly IConfigurationProvider _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookService"/> class.
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="mapper">AutoMapper for entities and dtos mapping</param>
        /// <param name="logger">ILogger to log errors</param>
        public BookService(ApplicationDBContext context, IMapper mapper, ILogger<BookService> logger)
        {
            _context = context;
            _mapper = mapper;
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
            var query = _context.Books.ProjectTo<BookDto>(_configuration);

            if (!string.IsNullOrEmpty(filter.Title))
            {
                query = query.Where(x => x.Title.ToLower().Contains(filter.Title.ToLower()));
            }

            if (!string.IsNullOrEmpty(filter.ISBN))
            {
                query = query.Where(x => x.ISBN.Contains(filter.ISBN));
            }

            if (filter.Rating != null)
            {
                query = query.Where(x => x.Rating >= filter.Rating);
            }

            if (filter.PublicationDateStartPeriod != null)
            {
                query = query.Where(x => x.PublicationDate >= filter.PublicationDateStartPeriod);
            }

            if (filter.PublicationDateEndPeriod != null)
            {
                query = query.Where(x => x.PublicationDate <= filter.PublicationDateEndPeriod);
            }

            return await query.ToListAsync();
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
                var updateBook = _mapper.Map(updateBookDto, bookEntity);

                _context.Books.Update(updateBook);

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
                    BookAuthors = x.BookAuthors
                })
                .FirstOrDefaultAsync();
        }
    }
}