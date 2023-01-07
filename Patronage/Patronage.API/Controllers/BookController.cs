using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Patronage.Application.Filters;
using Patronage.Application.Models.Book;
using Patronage.Application.Repositories;

namespace Patronage.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : Controller
    {
        private IValidator<CreateBookDto> _createBookValidator;
        private IValidator<UpdateBookDto> _updateBookValidator;
        private readonly IBookService _bookService;

        public BookController(IValidator<CreateBookDto> createBookValidator, IValidator<UpdateBookDto> updateBookValidator, IBookService bookService)
        {
            _createBookValidator = createBookValidator;
            _updateBookValidator = updateBookValidator;
            _bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
        }

        [HttpPost]
        public async Task<ActionResult<BookDto>> CreateBook(CreateBookDto createBookDto)
        {
            await _createBookValidator.ValidateAndThrowAsync(createBookDto);

            var bookDto = await _bookService.AddBookAsync(createBookDto);

            return Ok(bookDto);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks()
        {
            var booksDto = await _bookService.GetBooksAsync();

            return Ok(booksDto);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateBook(UpdateBookDto updateBookDto)
        {
            await _updateBookValidator.ValidateAndThrowAsync(updateBookDto);

            await _bookService.UpdateBookAsync(updateBookDto);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBook(int id)
        {
            await _bookService.DeleteBookAsync(id);

            return NoContent();
        }

        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetFilterdBook([FromQuery] BookFilter filter)
        {
            var bookDtos = await _bookService.GetFilteredBookAsync(filter);

            return Ok(bookDtos);
        }
    }
}