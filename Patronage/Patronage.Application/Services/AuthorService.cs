using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Patronage.Application.Filters;
using Patronage.Application.Models.Author;
using Patronage.Database;
using Patronage.Database.Entities;

namespace Patronage.Application.Repositories
{
    public class AuthorService : IAuthorService
    {
        private readonly ApplicationDBContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<BookService> _logger;
        private readonly IConfigurationProvider _configuration;


        public AuthorService(ApplicationDBContext context, IMapper mapper, ILogger<BookService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger;
            _configuration = _mapper.ConfigurationProvider;
        }

        // <inheritdoc />
        public async Task<AuthorDto> AddAuthorAsync(CreateAuthorDto createAuthorDto)
        {
            var authorEntity = _mapper.Map<Author>(createAuthorDto);

            using var transaction = _context.Database.BeginTransaction();

            try
            {
                _context.Authors.Add(authorEntity);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                transaction.Rollback();
                _logger.LogError(e.Message);
                throw;
            }

            return _mapper.Map<AuthorDto>(authorEntity);
        }

        // <inheritdoc />
        public async Task<HashSet<int>> AuthorsExist(List<int> ids)
        {
            return (await _context.Authors.Where(x => ids.Contains(x.Id)).Select(x => x.Id).ToListAsync()).ToHashSet();
        }

        // <inheritdoc />
        public async Task<IEnumerable<AuthorDto>> GetAuthorsAsync()
        {
            return await _context.Authors
                .Include(a => a.BookAuthors)
                .ProjectTo<AuthorDto>(_configuration)
                .ToListAsync();
        }

        // <inheritdoc />
        public async Task<IEnumerable<AuthorDto>> GetFilteredAuthorsAsync(AuthorFilter authorFilter)
        {
            return await _context.Authors
                .Where(a => (a.FirstName + " " + a.LastName).ToLower().Contains(authorFilter.Name.ToLower()))
                .ProjectTo<AuthorDto>(_configuration)
                .ToListAsync();
        }
    }
}