namespace Patronage.Application.Models.Book
{
    public class BaseBookDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Rating { get; set; }
        public string ISBN { get; set; }
        public DateTime PublicationDate { get; set; }
    }
}
