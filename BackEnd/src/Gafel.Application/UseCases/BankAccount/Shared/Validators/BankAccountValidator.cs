using FluentValidation;
using Gafel.Application.UseCases.BankAccount.Shared.Commands;
using Gafel.Domain.Resources;

namespace Gafel.Application.UseCases.BankAccount.Shared.Validators;

public class BankAccountValidator : AbstractValidator<BankAccountCommand>
{
    public BankAccountValidator()
    {
        RuleFor(bankAccount => bankAccount.Name)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.BANK_ACCOUNT_NAME_EMPTY)
            .MaximumLength(100)
            .WithMessage(ResourceMessagesException.BANK_ACCOUNT_NAME_TOO_LONG);

        RuleFor(bankAccount => bankAccount.InitialBalance)
            .GreaterThanOrEqualTo(0)
            .WithMessage(ResourceMessagesException.INVALID_INITIAL_BALANCE);

        RuleFor(bankAccount => bankAccount.Type)
            .IsInEnum()
            .WithMessage(ResourceMessagesException.BANK_ACCOUNT_TYPE_INVALID);
    }
}
