using FluentValidation;
using Gafel.Application.UseCases.Category.Shared.Commands;
using Gafel.Domain.Resources;

namespace Gafel.Application.UseCases.Category.Shared.Validators;
public class CategoryValidator : AbstractValidator<CategoryCommand>
{
    public CategoryValidator()
    {
        RuleFor(category => category.Name)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.CATEGORY_NAME_EMPTY)
            .MaximumLength(100)
            .WithMessage(ResourceMessagesException.CATEGORY_NAME_TOO_LONG);

        RuleFor(category => category.Icon)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.CATEGORY_ICON_EMPTY)
            .MaximumLength(50)
            .WithMessage(ResourceMessagesException.CATEGORY_ICON_TOO_LONG);

        RuleFor(category => category.Type)
            .IsInEnum()
            .WithMessage(ResourceMessagesException.CATEGORY_TYPE_INVALID);
    }
}
