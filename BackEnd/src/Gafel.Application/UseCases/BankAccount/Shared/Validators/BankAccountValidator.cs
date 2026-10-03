using FluentValidation;
using Gafel.Application.UseCases.BankAccount.Shared.Commands;
using Gafel.Domain.Constants;
using Gafel.Domain.Resources;

namespace Gafel.Application.UseCases.BankAccount.Shared.Validators;

public class BankAccountValidator : AbstractValidator<BankAccountCommand>
{
    public BankAccountValidator()
    {
        RuleFor(bankAccount => bankAccount.Name)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.NAME_REQUIRED)
            .MaximumLength(100)
            .WithMessage(ResourceMessagesException.BANK_ACCOUNT_NAME_TOO_LONG);

        RuleFor(bankAccount => bankAccount.InitialBalance)
            .InclusiveBetween(0, DomainRules.MaximumMoneyAmount)
            .WithMessage(ResourceMessagesException.BANK_ACCOUNT_INITIAL_BALANCE_OUT_OF_RANGE);

        RuleFor(bankAccount => bankAccount.Type)
            .IsInEnum()
            .WithMessage(ResourceMessagesException.TYPE_INVALID);
    }
}
