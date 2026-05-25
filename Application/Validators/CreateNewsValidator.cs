using FluentValidation;
using PWA_API.Application.DTOs.News;

namespace PWA_API.Application.Validators;

public class CreateNewsValidator : AbstractValidator<CreateNewsDto>
{
    public CreateNewsValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Content).NotEmpty();
        RuleFor(x => x.ImageUrl).MaximumLength(500).When(x => x.ImageUrl != null);
    }
}
