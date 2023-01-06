using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Patronage.Application.Filters;
using Patronage.Application.Models.Book;
using Patronage.Application.Repositories;
using Patronage.Database.Entities;

namespace Patronage.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : Controller
    {
        private IValidator<CreateBookDto> _createBookValidator;
        private IValidator<UpdateBookDto> _updateBookValidator;
        private readonly IBookService _bookRepository;
        private readonly IMapper _mapper;

        public BookController(IValidator<CreateBookDto> createBookValidator, IValidator<UpdateBookDto> updateBookValidator, IBookService bookRepository, IMapper mapper)
        {
            _createBookValidator = createBookValidator;
            _updateBookValidator = updateBookValidator;
            _bookRepository = bookRepository ??
                throw new ArgumentNullException(nameof(bookRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost]
        public async Task<ActionResult<BookDto>> CreateBook(CreateBookDto createBookDto)
        {
            var bookEntity = _mapper.Map<Book>(createBookDto);
            bookEntity.BookAuthors = new List<BookAuthor>();

            createBookDto.AuthorsIds = createBookDto.AuthorsIds.Distinct().ToList();

            foreach (int id in createBookDto.AuthorsIds)
            {
                if (!await _bookRepository.AuthorExistAsync(id))
                {
                    return NotFound("Author not found.");
                }
                BookAuthor bookAuthor = new BookAuthor
                {
                    AuthorId = id,
                };

                bookEntity.BookAuthors.Add(bookAuthor);
            }

            if (!(await _bookRepository.AddBookAsync(bookEntity)))
            {
                return BadRequest();
            }

            var bookDto = _mapper.Map<BookDto>(bookEntity);

            return Ok(bookDto);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks()
        {
            var bookEntities = await _bookRepository.GetBooksAsync();

            if (!bookEntities.Any())
            {
                return NotFound();
            }

            var booksDto = _mapper.Map<IEnumerable<BookDto>>(bookEntities);

            return Ok(booksDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateBook(int bookId, UpdateBookDto updateBookDto)
        {
            ValidationResult result = await _updateBookValidator.ValidateAsync(updateBookDto);

            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
                return BadRequest(errorMessages);
            }
            var bookEntity = await _bookRepository.GetBookAsync(bookId);

            if (bookEntity == null)
            {
                return NotFound();
            }

            _mapper.Map(updateBookDto, bookEntity);

            bookEntity.BookAuthors = new List<BookAuthor>();

            updateBookDto.AuthorsIds = updateBookDto.AuthorsIds.Distinct().ToList();

            foreach (int id in updateBookDto.AuthorsIds)
            {
                if (!await _bookRepository.AuthorExistAsync(id))
                {
                    return NotFound("Author not found.");
                }
                BookAuthor bookAuthor = new BookAuthor
                {
                    AuthorId = id,
                };

                bookEntity.BookAuthors.Add(bookAuthor);
            }

            if (!(await _bookRepository.SaveChangesAsync()))
            {
                return BadRequest();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBook(int bookId)
        {
            var bookEntity = await _bookRepository.GetBookAsync(bookId);

            if (bookEntity == null)
            {
                return NotFound();
            }

            if (!(await _bookRepository.DeleteBookAsync(bookEntity)))
            {
                return BadRequest();
            }
            return NoContent();
        }

        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetFilterdBook([FromQuery] BookFilter filter)
        {
            var bookEntities = await _bookRepository.GetFilteredBookAsync(filter);

            if (!bookEntities.Any())
            {
                return NotFound();
            }

            var booksDto = _mapper.Map<IEnumerable<BookDto>>(bookEntities);

            return Ok(booksDto);
        }
    }
}
