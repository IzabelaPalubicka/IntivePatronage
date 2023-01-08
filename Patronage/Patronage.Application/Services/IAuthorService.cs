using Patronage.Application.Filters;
using Patronage.Application.Models.Author;

namespace Patronage.Application.Repositories
{
    public interface IAuthorService
    {
        /// <summary>
        /// Add author
        /// </summary>
        /// <param name="createAuthorDto">The author to add</param>
        /// <returns>The dto of added author</returns>
        Task<AuthorDto> AddAuthorAsync(CreateAuthorDto createAuthorDto);

        /// <summary>
        /// Get all authors
        /// </summary>
        /// <returns>The dtos of received authors</returns>
        Task<IEnumerable<AuthorDto>> GetAuthorsAsync();

        /// <summary>
        /// Get filtered authors by filter params
        /// </summary>
        /// <param name="authorFilter">The filter params</param>
        /// <returns>The dtos of filtered authors</returns>
        Task<IEnumerable<AuthorDto>> GetFilteredAuthorsAsync(AuthorFilter filter);

        /// <summary>
        /// Check that authors exist by their id
        /// </summary>
        /// <param name="ids">Author ids list</param>
        /// <returns>The HashSet of existing authors ids</returns>
        Task<HashSet<int>> AuthorsExist(List<int> ids);
    }
}