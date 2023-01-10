using FluentValidation;
using Patronage.Application.Models.Author;

namespace Patronage.API.Validators.Authors
{
    public class CreateAuthorDtoValidator : AbstractValidator<CreateAuthorDto>
    {
        public CreateAuthorDtoValidator()
        {
            Include(new AuthorValidator());
        }
    }
}