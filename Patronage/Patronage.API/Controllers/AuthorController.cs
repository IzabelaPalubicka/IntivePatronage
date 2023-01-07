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

        public AuthorController(IValidator<CreateAuthorDto> createAuhorValidator, IAuthorService authorService)
        {
            _createAuthorValidator = createAuhorValidator;
            _authorService = authorService ??
                throw new ArgumentNullException(nameof(authorService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthors()
        {
            var authorDtos = await _authorService.GetAuthorsAsync();

            return Ok(authorDtos);
        }

        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> GetFilteredAuthors([FromQuery] AuthorFilter filter)
        {
            var authorDtos = await _authorService.GetFilteredAuthorsAsync(filter);

            return Ok(authorDtos);
        }

        [HttpPost]
        public async Task<ActionResult<AuthorDto>> CreateAuthor(CreateAuthorDto createAuthorDto)
        {
            await _createAuthorValidator.ValidateAndThrowAsync(createAuthorDto);

            var authorDto = await _authorService.AddAuthorAsync(createAuthorDto);

            return Ok(authorDto);
        }
    }
}