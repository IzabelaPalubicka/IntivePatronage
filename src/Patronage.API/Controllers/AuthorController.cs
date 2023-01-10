using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Patronage.Application.Filters;
using Patronage.Application.Models.Author;
using Patronage.Application.Services;

namespace Patronage.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorController : Controller
    {
        private readonly IValidator<CreateAuthorDto> _createAuthorValidator;
        private readonly IAuthorService _authorService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorController"/> class.
        /// </summary>
        /// <param name="createAuhorValidator">CreateAuthorDto validator</param>
        /// <param name="authorService">Service with author operations</param>
        public AuthorController(IValidator<CreateAuthorDto> createAuhorValidator, IAuthorService authorService)
        {
            _createAuthorValidator = createAuhorValidator;
            _authorService = authorService;
        }

        /// <summary>
        /// Get all authors
        /// </summary>
        /// <returns>The dtos of received authors</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthors()
        {
            var authorDtos = await _authorService.GetAuthorsAsync();

            return Ok(authorDtos);
        }

        /// <summary>
        /// Get filtered authors by filter params
        /// </summary>
        /// <param name="filter">The filter params</param>
        /// <returns>The dtos of filtered authors</returns>
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> GetFilteredAuthors([FromQuery] AuthorFilter filter)
        {
            var authorDtos = await _authorService.GetFilteredAuthorsAsync(filter);

            return Ok(authorDtos);
        }

        /// <summary>
        /// Create an author
        /// </summary>
        /// <param name="createAuthorDto">Author to add</param>
        /// <returns>The dto of the created author</returns>
        [HttpPost]
        public async Task<ActionResult<AuthorDto>> CreateAuthor(CreateAuthorDto createAuthorDto)
        {
            await _createAuthorValidator.ValidateAndThrowAsync(createAuthorDto);
            var authorDto = await _authorService.AddAuthorAsync(createAuthorDto);

            return Ok(authorDto);
        }
    }
}