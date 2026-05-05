using CommonTestUtilities.Dtos.QueryParams;
using FluentValidation.TestHelper;
using Gafel.Application.UseCases.Category.Filter;
using Gafel.Domain.Resources;
using Gafel.Domain.Enums;
using Shouldly;

namespace Validators.Test.Category.Filter;

public class FilterCategoryQueryParamsValidatorTest
{
    [Fact]
    public void Success()
    {
        // Arrange
        var request = FilterCategoryQueryParamsBuilder.Build();
        var validator = new FilterCategoryQueryParamsValidator();

        // Act
        var result = validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(-100)]
    [InlineData(-10)]
    [InlineData(0)]
    [InlineData(110)]
    public void Error_Limit_Out_Of_Range_1_To_100(int limit)
    {
        var request = FilterCategoryQueryParamsBuilder.Build(limit: limit);
        var validator = new FilterCategoryQueryParamsValidator();

        var result = validator.TestValidate(request);

        result.Errors.ShouldHaveSingleItem();
        result.ShouldHaveValidationErrorFor(x => x.Limit).WithErrorMessage(ResourceMessagesException.PAGINATION_LIMIT_RANGE);
    }

    [Theory]
    [InlineData(-100)]
    [InlineData(-10)]
    [InlineData(-1)]
    public void Error_Offset_Less_Than_Zero(int offset)
    {
        var request = FilterCategoryQueryParamsBuilder.Build(offset: offset);
        var validator = new FilterCategoryQueryParamsValidator();

        var result = validator.TestValidate(request);

        result.Errors.ShouldHaveSingleItem();
        result.ShouldHaveValidationErrorFor(x => x.Offset).WithErrorMessage(ResourceMessagesException.PAGINATION_OFFSET_MIN_VALUE);
    }


    [Fact]
    public void Error_Offset_Is_Null_And_Limit_Is_Valid()
    {
        var request = FilterCategoryQueryParamsBuilder.Build(nullOffset: true);
        var validator = new FilterCategoryQueryParamsValidator();

        var result = validator.TestValidate(request);

        result.Errors.ShouldHaveSingleItem();
        result.ShouldHaveValidationErrorFor(x => x.Offset).WithErrorMessage(ResourceMessagesException.PAGINATION_OFFSET_REQUIRED);
    }

    [Fact]
    public void Error_Limit_Is_Null_And_Offset_Is_Valid()
    {
        var request = FilterCategoryQueryParamsBuilder.Build(nullLimit: true);
        var validator = new FilterCategoryQueryParamsValidator();

        var result = validator.TestValidate(request);

        result.Errors.ShouldHaveSingleItem();
        result.ShouldHaveValidationErrorFor(x => x.Limit).WithErrorMessage(ResourceMessagesException.PAGINATION_LIMIT_REQUIRED);
    }


    [Fact]
    public void Error_Type_Invalid()
    {
        var request = FilterCategoryQueryParamsBuilder.Build(type: (CategoryType)100);
        var validator = new FilterCategoryQueryParamsValidator();

        var result = validator.TestValidate(request);

        result.Errors.ShouldHaveSingleItem();
        result.ShouldHaveValidationErrorFor(x => x.Type).WithErrorMessage(ResourceMessagesException.CATEGORY_TYPE_INVALID);
    }
}
