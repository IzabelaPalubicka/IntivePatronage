namespace Patronage.Application.Models.Book
{
    public class CreateBookDto : BaseBookDto
    {
        public List<int> AuthorsIds { get; set; }
    }
}
