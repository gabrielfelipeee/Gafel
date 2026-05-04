using FluentValidation;
using Gafel.Domain.Dtos.QueryParams;
using Gafel.Domain.Resources;

namespace Gafel.Application.UseCases.Shared.Validators;

public abstract class PaginationQueryParamsValidator<T> : AbstractValidator<T> where T : PaginationQueryParams
{
    public PaginationQueryParamsValidator()
    {
        RuleFor(x => x.Offset)
            .NotNull()
            .WithMessage(ResourceMessagesException.PAGINATION_OFFSET_REQUIRED)
            .GreaterThanOrEqualTo(0)
            .WithMessage(ResourceMessagesException.PAGINATION_OFFSET_MIN_VALUE);


        RuleFor(x => x.Limit)
            .NotNull()
            .WithMessage(ResourceMessagesException.PAGINATION_LIMIT_REQUIRED)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(100)
            .WithMessage(ResourceMessagesException.PAGINATION_LIMIT_RANGE);
    }
}
