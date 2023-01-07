using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Patronage.Application.Filters;
using Patronage.Application.Models.Author;
using Patronage.Application.Repositories;
using Patronage.Database.Entities;

namespace Patronage.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorController : Controller
    {
        private readonly IAuthorService _authorRepository;
        private readonly IMapper _mapper;
        public AuthorController(IAuthorService authorRepository, IMapper mapper)
        {
            _authorRepository = authorRepository ??
                throw new ArgumentNullException(nameof(authorRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthors()
        {
            var authorEntities = await _authorRepository.GetAuthorsAsync();

            if (!authorEntities.Any())
            {
                return NotFound();
            }

            var authorsDtoList = _mapper.Map<IEnumerable<AuthorDto>>(authorEntities);

            return Ok(authorsDtoList);
        }

        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> GetFilteredAuthors([FromQuery] AuthorFilter filter)
        {
            var authorEntities = await _authorRepository.GetFilteredAuthorsAsync(filter);

            if (!authorEntities.Any())
            {
                return NotFound();
            }

            var authorsDtoList = _mapper.Map<IEnumerable<AuthorDto>>(authorEntities);

            return Ok(authorsDtoList);
        }

        [HttpPost]
        public async Task<ActionResult<AuthorDto>> CreateAuthor(CreateAuthorDto createAuthorDto)
        {
            var authorEntitie = _mapper.Map<Author>(createAuthorDto);

            if (!(await _authorRepository.AddAuthorAsync(authorEntitie)))
            {
                return BadRequest();
            }

            var authorDto = _mapper.Map<AuthorDto>(authorEntitie);

            return Ok(authorDto);
        }
    }
}
