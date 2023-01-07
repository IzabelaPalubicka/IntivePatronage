using FluentValidation;
using Patronage.Application.Models.Book;
using Patronage.Application.Repositories;

namespace Patronage.API.Validators.Books
{
    public class UpdateBookDtoValidator : AbstractValidator<UpdateBookDto>
    {
        public UpdateBookDtoValidator(IAuthorService authorService, IBookService bookService)
        {
            Include(new BookValidator());
            RuleFor(x => x.Id).CustomAsync(async (dto, validationContext, cancellationToken) =>
            {
                var book = await bookService.GetBookAsync(dto);
                if (book == null)
                {
                    validationContext.AddFailure($"Book with id {dto} does not exist.");
                }
            });
            RuleFor(x => x.AuthorsIds).Custom((dtos, validationContext) =>
            {
                dtos.GroupBy(y => y)
                .Where(g => g.Count() > 1)
                .Select(z => $"Author with id {z} duplicated.")
                .ToList()
                .ForEach(x => validationContext.AddFailure(x));
            });
            RuleFor(x => x.AuthorsIds).CustomAsync(async (dtos, validationContext, cancellationToken) =>
            {
                var existingIds = await authorService.AuthorsExist(dtos);
                foreach (int authorId in dtos)
                {
                    if (!existingIds.Contains(authorId))
                    {
                        validationContext.AddFailure($"Author with id {authorId} does not exist.");
                    }
                }
            });
        }
    }
}