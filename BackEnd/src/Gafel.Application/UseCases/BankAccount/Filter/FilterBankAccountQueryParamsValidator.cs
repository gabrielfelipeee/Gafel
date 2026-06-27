using FluentValidation;
using Gafel.Application.UseCases.Shared.Validators;
using Gafel.Domain.Dtos.QueryParams;
using Gafel.Domain.Resources;

namespace Gafel.Application.UseCases.BankAccount.Filter;

public class FilterBankAccountQueryParamsValidator : OptionalPaginationQueryParamsValidator<FilterBankAccountQueryParams>
{
    public FilterBankAccountQueryParamsValidator()
    {
        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage(ResourceMessagesException.BANK_ACCOUNT_TYPE_INVALID)
            .When(x => x.Type.HasValue);
    }
}
