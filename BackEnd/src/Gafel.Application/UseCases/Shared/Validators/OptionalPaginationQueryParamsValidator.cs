using FluentValidation;
using Gafel.Domain.Dtos.QueryParams;
using Gafel.Domain.Resources;

namespace Gafel.Application.UseCases.Shared.Validators;

public class OptionalPaginationQueryParamsValidator<T> : AbstractValidator<T> where T : OptionalPaginationQueryParams
{
    protected OptionalPaginationQueryParamsValidator()
    {
        RuleFor(x => x.Offset)
            .GreaterThanOrEqualTo(0)
            .WithMessage(ResourceMessagesException.PAGINATION_OFFSET_MIN_VALUE)
            .When(x => x.Offset.HasValue);

        RuleFor(x => x.Limit)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(100)
            .WithMessage(ResourceMessagesException.PAGINATION_LIMIT_RANGE)
            .When(x => x.Limit.HasValue);


        //  Manda os dois ou nenhum
        RuleFor(x => x.Offset)
            .NotNull()
            .WithMessage(ResourceMessagesException.PAGINATION_OFFSET_REQUIRED)
            .When(x => x.Limit.HasValue);

        RuleFor(x => x.Limit)
            .NotNull()
            .WithMessage(ResourceMessagesException.PAGINATION_LIMIT_REQUIRED)
            .When(x => x.Offset.HasValue);
    }
}
