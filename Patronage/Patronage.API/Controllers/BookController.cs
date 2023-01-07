using AutoMapper;
using FluentValidation;
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
            _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost]
        public async Task<ActionResult<BookDto>> CreateBook(CreateBookDto createBookDto)
        {
            await _createBookValidator.ValidateAndThrowAsync(createBookDto);

            var bookEntity = _mapper.Map<Book>(createBookDto);

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
        public async Task<ActionResult> UpdateBook(int id, UpdateBookDto updateBookDto)
        {
            await _updateBookValidator.ValidateAndThrowAsync(updateBookDto);

            if (!(await _bookRepository.UpdateBookAsync(id, updateBookDto)))
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBook(int id)
        {
            var bookEntity = await _bookRepository.GetBookAsync(id);

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
