namespace Patronage.Application.Models.Book
{
    public class UpdateBookDto : BaseBookDto
    {
        public int Id { get; set; }
        public List<int> AuthorsIds { get; set; }
    }
}