using CommonTestUtilities.Commands;
using FluentValidation.TestHelper;
using Gafel.Application.UseCases.BankAccount.Shared.Validators;
using Gafel.Domain.Enums;
using Gafel.Domain.Resources;
using Shouldly;

namespace Validators.Test.BankAccount.Create;

public class CreateBankAccountValidatorTest
{
    [Fact]
    public void Success()
    {
        // Arrange
        var request = BankAccountCommandBuilder.Build();
        var validator = new BankAccountValidator();

        // Act
        var result = validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Error_Name_Empty()
    {
        var request = BankAccountCommandBuilder.Build();
        request.Name = string.Empty;
        var validator = new BankAccountValidator();

        var result = validator.TestValidate(request);

        result.Errors.ShouldHaveSingleItem();
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage(ResourceMessagesException.NAME_REQUIRED);
    }

    [Fact]
    public void Error_InitialBalance_Negative()
    {
        var request = BankAccountCommandBuilder.Build();
        request.InitialBalance = -100;
        var validator = new BankAccountValidator();

        var result = validator.TestValidate(request);

        result.Errors.ShouldHaveSingleItem();
        result.ShouldHaveValidationErrorFor(x => x.InitialBalance)
            .WithErrorMessage(ResourceMessagesException.BANK_ACCOUNT_INITIAL_BALANCE_OUT_OF_RANGE);
    }

    [Fact]
    public void Error_Type_Invalid()
    {
        var request = BankAccountCommandBuilder.Build();
        request.Type = (BankAccountType)100;
        var validator = new BankAccountValidator();

        var result = validator.TestValidate(request);

        result.Errors.ShouldHaveSingleItem();
        result.ShouldHaveValidationErrorFor(x => x.Type)
            .WithErrorMessage(ResourceMessagesException.TYPE_INVALID);
    }
}
