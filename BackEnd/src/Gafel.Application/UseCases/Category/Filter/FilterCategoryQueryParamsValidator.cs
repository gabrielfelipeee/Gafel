using FluentValidation;
using Gafel.Application.UseCases.Shared.Validators;
using Gafel.Domain.Dtos.QueryParams;
using Gafel.Domain.Resources;

namespace Gafel.Application.UseCases.Category.Filter;

public class FilterCategoryQueryParamsValidator : OptionalPaginationQueryParamsValidator<FilterCategoryQueryParams>
{
    public FilterCategoryQueryParamsValidator()
    {
        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage(ResourceMessagesException.CATEGORY_TYPE_INVALID)
            .When(x => x.Type.HasValue);
    }
}
