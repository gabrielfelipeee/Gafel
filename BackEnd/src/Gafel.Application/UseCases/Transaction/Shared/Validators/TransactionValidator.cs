using FluentValidation;
using Gafel.Application.UseCases.Transaction.Shared.Commands;
using Gafel.Domain.Constants;
using Gafel.Domain.Resources;

namespace Gafel.Application.UseCases.Transaction.Shared.Validators;

public class TransactionValidator : AbstractValidator<TransactionCommand>
{
    public TransactionValidator()
    {
        RuleFor(transaction => transaction.Description)
            .MaximumLength(100)
            .WithMessage(ResourceMessagesException.TRANSACTION_DESCRIPTION_TOO_LONG);

        RuleFor(transaction => transaction.Amount)
            .InclusiveBetween(0.01m, DomainRules.MaximumMoneyAmount)
            .WithMessage(ResourceMessagesException.TRANSACTION_AMOUNT_OUT_OF_RANGE);

        RuleFor(transaction => transaction.Date)
            .NotEqual(default(DateOnly))
            .WithMessage(ResourceMessagesException.TRANSACTION_DATE_REQUIRED);

        RuleFor(transaction => transaction.BankAccountId)
            .NotEqual(Guid.Empty)
            .WithMessage(ResourceMessagesException.TRANSACTION_BANK_ACCOUNT_REQUIRED);

        RuleFor(transaction => transaction.CategoryId)
            .NotEqual(Guid.Empty)
            .WithMessage(ResourceMessagesException.TRANSACTION_CATEGORY_REQUIRED);
    }
}
