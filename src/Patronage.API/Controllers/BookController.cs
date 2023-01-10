using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Patronage.Application.Filters;
using Patronage.Application.Models.Book;
using Patronage.Application.Services;

namespace Patronage.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : Controller
    {
        private readonly IValidator<CreateBookDto> _createBookValidator;
        private readonly IValidator<UpdateBookDto> _updateBookValidator;
        private readonly IBookService _bookService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookController"/> class.
        /// </summary>
        /// <param name="createBookValidator">CreateBookDto validator</param>
        /// <param name="updateBookValidator">UpdateBookDto validator</param>
        /// <param name="bookService">Service with book operations</param>
        public BookController(IValidator<CreateBookDto> createBookValidator, IValidator<UpdateBookDto> updateBookValidator, IBookService bookService)
        {
            _createBookValidator = createBookValidator;
            _updateBookValidator = updateBookValidator;
            _bookService = bookService;
        }

        /// <summary>
        /// Create a book
        /// </summary>
        /// <param name="createBookDto">The book to add</param>
        /// <returns>The dto of created book</returns>
        [HttpPost]
        public async Task<ActionResult<BookDto>> CreateBook(CreateBookDto createBookDto)
        {
            await _createBookValidator.ValidateAndThrowAsync(createBookDto);

            var bookDto = await _bookService.AddBookAsync(createBookDto);

            return Ok(bookDto);
        }

        /// <summary>
        /// Get all books
        /// </summary>
        /// <returns>The dtos of received books</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks()
        {
            var booksDto = await _bookService.GetBooksAsync();

            return Ok(booksDto);
        }

        /// <summary>
        /// Update a book
        /// </summary>
        /// <param name="updateBookDto">The book data to update</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult> UpdateBook(UpdateBookDto updateBookDto)
        {
            await _updateBookValidator.ValidateAndThrowAsync(updateBookDto);

            await _bookService.UpdateBookAsync(updateBookDto);

            return NoContent();
        }

        /// <summary>
        /// Delete a book by id
        /// </summary>
        /// <param name="id">The id of the book to delete</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBook(int id)
        {
            await _bookService.DeleteBookAsync(id);

            return NoContent();
        }

        /// <summary>
        /// Get filtered books by filtered params
        /// </summary>
        /// <param name="filter">The filter params</param>
        /// <returns>The dtos of the filtered books</returns>
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetFilterdBook([FromQuery] BookFilter filter)
        {
            var bookDtos = await _bookService.GetFilteredBooksAsync(filter);

            return Ok(bookDtos);
        }
    }
}