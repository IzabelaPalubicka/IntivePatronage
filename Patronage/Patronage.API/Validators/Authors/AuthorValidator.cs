using FluentValidation;
using Patronage.Application.Models.Author;

namespace Patronage.API.Validators.Authors
{
    public class AuthorValidator : AbstractValidator<BaseAuthorDto>
    {
        public AuthorValidator()
        {
            RuleFor(a => a.FirstName).NotEmpty().MaximumLength(50);
            RuleFor(a => a.LastName).NotEmpty().MaximumLength(50);
            RuleFor(a => a.BirthDate).NotEmpty();
            RuleFor(a => a.Gender).NotNull();
        }
    }
}