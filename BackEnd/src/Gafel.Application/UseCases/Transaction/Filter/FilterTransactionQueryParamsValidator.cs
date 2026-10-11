using FluentValidation;
using Gafel.Application.UseCases.Shared.Validators;
using Gafel.Domain.Constants;
using Gafel.Domain.Dtos.QueryParams;
using Gafel.Domain.Resources;

namespace Gafel.Application.UseCases.Transaction.Filter;

public class FilterTransactionQueryParamsValidator : PaginationQueryParamsValidator<FilterTransactionQueryParams>
{
    public FilterTransactionQueryParamsValidator()
    {
        RuleFor(x => x.EndDate)
            .GreaterThanOrEqualTo(x => x.StartDate!.Value)
            .WithMessage(ResourceMessagesException.TRANSACTION_END_DATE_BEFORE_START_DATE)
            .When(x => x.StartDate.HasValue && x.EndDate.HasValue);

        RuleFor(x => x.EndDate)
            .Must((query, endDate) => !query.StartDate.HasValue || !endDate.HasValue || endDate.Value <= query.StartDate.Value.AddYears(1))
            .WithMessage(ResourceMessagesException.TRANSACTION_DATE_RANGE_EXCEEDS_ONE_YEAR);

        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage(ResourceMessagesException.TYPE_INVALID)
            .When(x => x.Type.HasValue);

        RuleFor(x => x.MinimumAmount)
            .InclusiveBetween(DomainRules.MinimumTransactionAmount, DomainRules.MaximumMoneyAmount)
            .WithMessage(ResourceMessagesException.TRANSACTION_MINIMUM_AMOUNT_OUT_OF_RANGE)
            .When(x => x.MinimumAmount.HasValue);

        RuleFor(x => x.MinimumAmount)
            .LessThanOrEqualTo(x => x.MaximumAmount!.Value)
            .WithMessage(ResourceMessagesException.TRANSACTION_MINIMUM_AMOUNT_GREATER_THAN_MAXIMUM)
            .When(x => x.MinimumAmount.HasValue && x.MaximumAmount.HasValue);

        RuleFor(x => x.MaximumAmount)
            .InclusiveBetween(DomainRules.MinimumTransactionAmount, DomainRules.MaximumMoneyAmount)
            .WithMessage(ResourceMessagesException.TRANSACTION_MAXIMUM_AMOUNT_OUT_OF_RANGE)
            .When(x => x.MaximumAmount.HasValue);

        RuleFor(x => x.BankAccountId)
            .NotEqual(Guid.Empty)
            .WithMessage(ResourceMessagesException.ID_INVALID)
            .When(x => x.BankAccountId.HasValue);

        RuleFor(x => x.CategoryId)
            .NotEqual(Guid.Empty)
            .WithMessage(ResourceMessagesException.ID_INVALID)
            .When(x => x.CategoryId.HasValue);
    }
}
