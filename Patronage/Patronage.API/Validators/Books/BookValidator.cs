using FluentValidation;
using Patronage.Application.Models.Book;

namespace Patronage.API.Validators.Books
{
    public class BookValidator : AbstractValidator<BaseBookDto>
    {
        public BookValidator()
        {
            RuleFor(b => b.Title).NotEmpty().MaximumLength(100).WithMessage("{PropertyName} cannot be empty string and the maximum length is 100.");
            RuleFor(b => b.Description).NotEmpty().WithMessage("{PropertyName} cannot be empty string.");
            RuleFor(b => b.Rating).ScalePrecision(2, 4, false).InclusiveBetween(0, 10).WithMessage("{PropertyName} cannot be empty bool.");
            RuleFor(b => b.ISBN).NotEmpty().MaximumLength(13).WithMessage("{PropertyName} cannot be empty string and the maximum length is 13.");
            RuleFor(b => b.PublicationDate).NotEmpty().WithMessage("{PropertyName} cannot be empty DateTime.");
        }
    }
}
