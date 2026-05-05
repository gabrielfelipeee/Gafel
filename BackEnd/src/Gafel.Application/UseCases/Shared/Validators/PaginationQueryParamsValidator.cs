using FluentValidation;
using Gafel.Domain.Dtos.QueryParams;
using Gafel.Domain.Resources;

namespace Gafel.Application.UseCases.Shared.Validators;

public class PaginationQueryParamsValidator<T> : AbstractValidator<T> where T : PaginationQueryParams
{
    protected PaginationQueryParamsValidator()
    {
        RuleFor(x => x.Offset)
            .NotNull()
            .WithMessage(ResourceMessagesException.PAGINATION_OFFSET_REQUIRED)
            .GreaterThanOrEqualTo(0)
            .WithMessage(ResourceMessagesException.PAGINATION_OFFSET_MIN_VALUE);


        RuleFor(x => x.Limit)
            .NotNull()
            .WithMessage(ResourceMessagesException.PAGINATION_LIMIT_REQUIRED)
            .InclusiveBetween(1, 100)
            .WithMessage(ResourceMessagesException.PAGINATION_LIMIT_RANGE);
    }
}
