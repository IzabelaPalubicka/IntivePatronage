using FluentValidation;
using Patronage.Application.Models.Book;
using Patronage.Application.Repositories;

namespace Patronage.API.Validators.Books
{
    public class CreateBookDtoValidator : AbstractValidator<CreateBookDto>
    {
        public CreateBookDtoValidator(IAuthorService authorService)
        {
            Include(new BookValidator());
            RuleFor(x => x.AuthorsIds).CustomAsync(async (dtos, validationContext, cancellationToken) =>
            {
                dtos.GroupBy(y => y)
                    .Where(g => g.Count() > 1)
                    .Select(z => $"Author with id {z} duplicated.")
                    .ToList()
                    .ForEach(x => validationContext.AddFailure(x));

                var existingIds = await authorService.AuthorsExist(dtos);
                foreach (int authorId in dtos)
                {
                    if (!existingIds.Contains(authorId))
                    {
                        validationContext.AddFailure(new FluentValidation.Results.ValidationFailure("shu", $"Author with id {authorId} does not exist."));
                    }
                }
            });
        }
    }
}