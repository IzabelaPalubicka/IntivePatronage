using Patronage.Application.Filters;
using Patronage.Database.Entities;

namespace Patronage.Application.Repositories
{
    public interface IAuthorService
    {
        Task<bool> AddAuthorAsync(Author author);
        Task<IEnumerable<Author>> GetAuthorsAsync();
        Task<IEnumerable<Author>> GetFilteredAuthorsAsync(AuthorFilter filter);
        Task<bool> SaveChangesAsync();

    }
}
