namespace Patronage.Application.Models.Book
{
    public class BaseBookDto
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Rating { get; set; }
        public string ISBN { get; set; } = null!;
        public DateTime PublicationDate { get; set; }
    }
}