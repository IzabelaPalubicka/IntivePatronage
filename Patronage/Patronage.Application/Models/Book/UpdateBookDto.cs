namespace Patronage.Application.Models.Book
{
    public class UpdateBookDto : BaseBookDto
    {
        public List<int> AuthorsIds { get; set; }
    }
}
