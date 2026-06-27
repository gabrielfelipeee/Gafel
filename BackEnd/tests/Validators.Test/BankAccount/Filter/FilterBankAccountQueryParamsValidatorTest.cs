using CommonTestUtilities.Dtos.QueryParams;
using FluentValidation.TestHelper;
using Gafel.Application.UseCases.BankAccount.Filter;
using Gafel.Domain.Enums;
using Gafel.Domain.Resources;
using Shouldly;

namespace Validators.Test.BankAccount.Filter;

public class FilterBankAccountQueryParamsValidatorTest
{
    [Fact]
    public void Success()
    {
        // Arrange
        var request = FilterBankAccountQueryParamsBuilder.Build();
        var validator = new FilterBankAccountQueryParamsValidator();

        // Act
        var result = validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Error_Type_Invalid()
    {
        var request = FilterBankAccountQueryParamsBuilder.Build(type: (BankAccountType)100);
        var validator = new FilterBankAccountQueryParamsValidator();

        var result = validator.TestValidate(request);

        result.Errors.ShouldHaveSingleItem();
        result.ShouldHaveValidationErrorFor(x => x.Type).WithErrorMessage(ResourceMessagesException.BANK_ACCOUNT_TYPE_INVALID);
    }
}
