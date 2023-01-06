using Microsoft.EntityFrameworkCore;
using Patronage.Application.Filters;
using Patronage.Database;
using Patronage.Database.Entities;

namespace Patronage.Application.Repositories
{
    public class AuthorService : IAuthorService
    {
        private readonly ApplicationDBContext _context;
        public AuthorService(ApplicationDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> AddAuthorAsync(Author author)
        {
            if (author is null)
            {
                throw new ArgumentNullException(nameof(author));
            }

            try
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    _context.Authors.Add(author);

                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public async Task<IEnumerable<Author>> GetAuthorsAsync()
        {
            return await _context.Authors.Include(a => a.BookAuthors).ToListAsync();
        }

        public async Task<IEnumerable<Author>> GetFilteredAuthorsAsync(AuthorFilter authorFilter)
        {
            return await _context.Authors
                .Where(a => (a.FirstName + " " + a.LastName).ToLower().Contains(authorFilter.Name.ToLower()))
                .ToListAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}
