using FluentValidation;
using Patronage.Application.Models.Author;

namespace Patronage.API.Validators.Authors
{
    public class AuthorValidator : AbstractValidator<BaseAuthorDto>
    {
        public AuthorValidator()
        {
            RuleFor(a => a.FirstName).NotEmpty().MaximumLength(50).WithMessage("{PropertyName} cannot be empty string and the maximum length is 50.");
            RuleFor(a => a.LastName).NotEmpty().MaximumLength(50).WithMessage("{PropertyName} cannot be empty string and the maximum length is 50.");
            RuleFor(a => a.BirthDate).NotEmpty().WithMessage("{PropertyName} cannot be empty DateTime.");
            RuleFor(a => a.Gender).NotNull().WithMessage("{PropertyName} must be a bool and cannot be null.");
        }
    }
}