using FluentValidation;
using Patronage.Application.Models.Book;

namespace Patronage.API.Validators.Books
{
    public class BookValidator : AbstractValidator<BaseBookDto>
    {
        public BookValidator()
        {
            RuleFor(b => b.Title).NotEmpty().MaximumLength(100);
            RuleFor(b => b.Description).NotEmpty();
            RuleFor(b => b.Rating).ScalePrecision(2, 4, false).InclusiveBetween(0, 10);
            RuleFor(b => b.ISBN).NotEmpty().MaximumLength(13);
            RuleFor(b => b.PublicationDate).NotEmpty();
        }
    }
}