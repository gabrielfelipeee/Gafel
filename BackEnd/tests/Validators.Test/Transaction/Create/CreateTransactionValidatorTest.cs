using CommonTestUtilities.Commands;
using FluentValidation.TestHelper;
using Gafel.Application.UseCases.Transaction.Shared.Validators;
using Gafel.Domain.Constants;
using Gafel.Domain.Resources;
using Shouldly;

namespace Validators.Test.Transaction.Create;

public class CreateTransactionValidatorTest
{
    [Fact]
    public void Success()
    {
        // Arrange
        var request = TransactionCommandBuilder.Build();
        var validator = new TransactionValidator();

        // Act
        var result = validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Error_BankAccountId_Empty()
    {
        var request = TransactionCommandBuilder.Build(bankAccountId: Guid.Empty);
        var validator = new TransactionValidator();

        var result = validator.TestValidate(request);

        result.Errors.ShouldHaveSingleItem();
        result.ShouldHaveValidationErrorFor(x => x.BankAccountId)
            .WithErrorMessage(ResourceMessagesException.TRANSACTION_BANK_ACCOUNT_REQUIRED);
    }

    [Fact]
    public void Error_CategoryId_Empty()
    {
        var request = TransactionCommandBuilder.Build(categoryId: Guid.Empty);
        var validator = new TransactionValidator();

        var result = validator.TestValidate(request);

        result.Errors.ShouldHaveSingleItem();
        result.ShouldHaveValidationErrorFor(x => x.CategoryId)
            .WithErrorMessage(ResourceMessagesException.TRANSACTION_CATEGORY_REQUIRED);
    }

    [Fact]
    public void Error_Amount_Zero()
    {
        var request = TransactionCommandBuilder.Build();
        request.Amount = 0m;
        var validator = new TransactionValidator();

        var result = validator.TestValidate(request);

        result.Errors.ShouldHaveSingleItem();
        result.ShouldHaveValidationErrorFor(x => x.Amount)
            .WithErrorMessage(ResourceMessagesException.TRANSACTION_AMOUNT_OUT_OF_RANGE);
    }

    [Fact]
    public void Error_Amount_AboveMaximum()
    {
        var request = TransactionCommandBuilder.Build();
        request.Amount = DomainRules.MaximumMoneyAmount + 1m;
        var validator = new TransactionValidator();

        var result = validator.TestValidate(request);

        result.Errors.ShouldHaveSingleItem();
        result.ShouldHaveValidationErrorFor(x => x.Amount)
            .WithErrorMessage(ResourceMessagesException.TRANSACTION_AMOUNT_OUT_OF_RANGE);
    }

    [Fact]
    public void Error_Date_Default()
    {
        var request = TransactionCommandBuilder.Build();
        request.Date = default;
        var validator = new TransactionValidator();

        var result = validator.TestValidate(request);

        result.Errors.ShouldHaveSingleItem();
        result.ShouldHaveValidationErrorFor(x => x.Date)
            .WithErrorMessage(ResourceMessagesException.TRANSACTION_DATE_REQUIRED);
    }
}
