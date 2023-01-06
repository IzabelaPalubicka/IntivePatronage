using Microsoft.EntityFrameworkCore;
using Patronage.Database.Entities;

namespace Patronage.Database
{
    public class ApplicationDBContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }

        public DbSet<BookAuthor> AuthorBooks { get; set; }

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
        : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookAuthor>()
                .HasKey(ba => new { ba.AuthorId, ba.BookId });
            modelBuilder.Entity<BookAuthor>()
                .HasOne(ba => ba.Author)
                .WithMany(a => a.BookAuthors)
                .HasForeignKey(ab => ab.AuthorId);
            modelBuilder.Entity<BookAuthor>()
                .HasOne(ba => ba.Book)
                .WithMany(b => b.BookAuthors)
                .HasForeignKey(ab => ab.BookId);
        }
    }
}
