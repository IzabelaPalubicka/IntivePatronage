using Patronage.Application.Filters;
using Patronage.Application.Models.Author;

namespace Patronage.Application.Repositories
{
    public interface IAuthorService
    {
        Task<AuthorDto> AddAuthorAsync(CreateAuthorDto createAuthorDto);

        Task<IEnumerable<AuthorDto>> GetAuthorsAsync();

        Task<IEnumerable<AuthorDto>> GetFilteredAuthorsAsync(AuthorFilter filter);

        Task<HashSet<int>> AuthorsExist(List<int> ids);
    }
}