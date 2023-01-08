using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Patronage.Application.Filters;
using Patronage.Application.Models.Author;
using Patronage.Application.Repositories;

namespace Patronage.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorController : Controller
    {
        private IValidator<CreateAuthorDto> _createAuthorValidator;
        private readonly IAuthorService _authorService;
        private readonly ILogger<AuthorController> _logger;

        public AuthorController(IValidator<CreateAuthorDto> createAuhorValidator, IAuthorService authorService, ILogger<AuthorController> logger)
        {
            _createAuthorValidator = createAuhorValidator;
            _authorService = authorService ?? throw new ArgumentNullException(nameof(authorService));
            _logger = logger;
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