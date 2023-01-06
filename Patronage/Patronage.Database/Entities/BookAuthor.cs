using System.ComponentModel.DataAnnotations.Schema;

namespace Patronage.Database.Entities
{
    public class BookAuthor
    {
        [ForeignKey("AuthorId")]
        public int AuthorId { get; set; }
        public Author Author { get; set; }

        [ForeignKey("BookId")]
        public int BookId { get; set; }
        public Book Book { get; set; }
    }
}
