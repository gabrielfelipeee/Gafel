using CommonTestUtilities.Dtos.QueryParams;
using FluentValidation.TestHelper;
using Gafel.Application.UseCases.Transaction.Filter;
using Gafel.Domain.Constants;
using Gafel.Domain.Enums;
using Gafel.Domain.Resources;
using Shouldly;

namespace Validators.Test.Transaction.Filter;

public class FilterTransactionQueryParamsValidatorTest
{
    [Fact]
    public void Success()
    {
        // Arrange
        var request = FilterTransactionQueryParamsBuilder.Build();
        var validator = new FilterTransactionQueryParamsValidator();

        // Act
        var result = validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Error_DateRange_ExceedsOneYear()
    {
        var request = FilterTransactionQueryParamsBuilder.Build();

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        request.StartDate = new DateOnly(today.Year, today.Month, 1);
        request.EndDate = request.StartDate.Value.AddYears(1).AddDays(1);

        var validator = new FilterTransactionQueryParamsValidator();

        var result = validator.TestValidate(request);

        result.Errors.ShouldHaveSingleItem();
        result.ShouldHaveValidationErrorFor(x => x.EndDate)
            .WithErrorMessage(ResourceMessagesException.TRANSACTION_DATE_RANGE_EXCEEDS_ONE_YEAR);
    }

    [Fact]
    public void Error_EndDate_BeforeStartDate()
    {
        var request = FilterTransactionQueryParamsBuilder.Build();

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        request.StartDate = new DateOnly(today.Year, today.Month, day: 10);
        request.EndDate = request.StartDate.Value.AddDays(-1);

        var validator = new FilterTransactionQueryParamsValidator();

        var result = validator.TestValidate(request);

        result.Errors.ShouldHaveSingleItem();
        result.ShouldHaveValidationErrorFor(x => x.EndDate)
            .WithErrorMessage(ResourceMessagesException.TRANSACTION_END_DATE_BEFORE_START_DATE);
    }

    [Fact]
    public void Error_Type_Invalid()
    {
        var request = FilterTransactionQueryParamsBuilder.Build();
        request.Type = (CategoryType)100;
        var validator = new FilterTransactionQueryParamsValidator();

        var result = validator.TestValidate(request);

        result.Errors.ShouldHaveSingleItem();
        result.ShouldHaveValidationErrorFor(x => x.Type).WithErrorMessage(ResourceMessagesException.TYPE_INVALID);
    }

    [Fact]
    public void Error_MaximumAmount_AboveMaximum()
    {
        var request = FilterTransactionQueryParamsBuilder.Build();
        request.MaximumAmount = DomainRules.MaximumMoneyAmount + 1m;
        var validator = new FilterTransactionQueryParamsValidator();

        var result = validator.TestValidate(request);

        result.Errors.ShouldHaveSingleItem();
        result.ShouldHaveValidationErrorFor(x => x.MaximumAmount)
            .WithErrorMessage(ResourceMessagesException.TRANSACTION_MAXIMUM_AMOUNT_OUT_OF_RANGE);
    }

    [Fact]
    public void Error_MinimumAmount_BelowMinimum()
    {
        var request = FilterTransactionQueryParamsBuilder.Build();
        request.MinimumAmount = DomainRules.MinimumTransactionAmount - 1m;
        var validator = new FilterTransactionQueryParamsValidator();

        var result = validator.TestValidate(request);

        result.Errors.ShouldHaveSingleItem();
        result.ShouldHaveValidationErrorFor(x => x.MinimumAmount)
            .WithErrorMessage(ResourceMessagesException.TRANSACTION_MINIMUM_AMOUNT_OUT_OF_RANGE);
    }

    [Fact]
    public void Error_MinimumAmount_AboveMaximum()
    {
        var request = FilterTransactionQueryParamsBuilder.Build();
        request.MinimumAmount = 11m;
        request.MaximumAmount = 10m;
        var validator = new FilterTransactionQueryParamsValidator();

        var result = validator.TestValidate(request);

        result.Errors.ShouldHaveSingleItem();
        result.ShouldHaveValidationErrorFor(x => x.MinimumAmount)
            .WithErrorMessage(ResourceMessagesException.TRANSACTION_MINIMUM_AMOUNT_GREATER_THAN_MAXIMUM);
    }

    [Fact]
    public void Error_BankAccountId_Empty()
    {
        var request = FilterTransactionQueryParamsBuilder.Build();
        request.BankAccountId = Guid.Empty;
        var validator = new FilterTransactionQueryParamsValidator();

        var result = validator.TestValidate(request);

        result.Errors.ShouldHaveSingleItem();
        result.ShouldHaveValidationErrorFor(x => x.BankAccountId)
            .WithErrorMessage(ResourceMessagesException.ID_INVALID);
    }

    [Fact]
    public void Error_CategoryId_Empty()
    {
        var request = FilterTransactionQueryParamsBuilder.Build();
        request.CategoryId = Guid.Empty;
        var validator = new FilterTransactionQueryParamsValidator();

        var result = validator.TestValidate(request);

        result.Errors.ShouldHaveSingleItem();
        result.ShouldHaveValidationErrorFor(x => x.CategoryId)
            .WithErrorMessage(ResourceMessagesException.ID_INVALID);
    }
}
