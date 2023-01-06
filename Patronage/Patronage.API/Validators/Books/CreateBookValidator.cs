using FluentValidation;
using Patronage.Application.Models.Book;
using Patronage.Database;

namespace Patronage.API.Validators.Books
{
    public class CreateBookValidator : AbstractValidator<CreateBookDto>
    {
        public CreateBookValidator(ApplicationDBContext context)
        {
            Include(new BookValidator());
        }
    }
}
