using CommonTestUtilities.Commands;
using FluentValidation.TestHelper;
using Gafel.Application.UseCases.Category.Shared.Validators;
using Gafel.Domain.Enums;
using Gafel.Domain.Resources;
using Shouldly;

namespace Validators.Test.Category.Register;

public class RegisterCategoryValidatorTest
{
    [Fact]
    public void Success()
    {
        // Arrange
        var request = CategoryCommandBuilder.Build();
        var validator = new CategoryValidator();

        // Act
        var result = validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Error_Name_Empty()
    {
        var request = CategoryCommandBuilder.Build();
        request.Name = string.Empty;
        var validator = new CategoryValidator();

        var result = validator.TestValidate(request);

        result.Errors.ShouldHaveSingleItem();
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage(ResourceMessagesException.CATEGORY_NAME_EMPTY);
    }

    [Fact]
    public void Error_Icon_Empty()
    {
        var request = CategoryCommandBuilder.Build();
        request.Icon = string.Empty;
        var validator = new CategoryValidator();

        var result = validator.TestValidate(request);

        result.Errors.ShouldHaveSingleItem();
        result.ShouldHaveValidationErrorFor(x => x.Icon)
            .WithErrorMessage(ResourceMessagesException.CATEGORY_ICON_EMPTY);
    }

    [Fact]
    public void Error_Type_Invalid()
    {
        var request = CategoryCommandBuilder.Build();
        request.Type = (CategoryType)100;
        var validator = new CategoryValidator();

        var result = validator.TestValidate(request);

        result.Errors.ShouldHaveSingleItem();
        result.ShouldHaveValidationErrorFor(x => x.Type)
            .WithErrorMessage(ResourceMessagesException.CATEGORY_TYPE_INVALID);
    }
}
