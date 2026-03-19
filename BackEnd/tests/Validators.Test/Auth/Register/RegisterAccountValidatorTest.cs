using CommonTestUtilities.Commands;
using FluentValidation.TestHelper;
using Gafel.Application.UseCases.Auth.Register;
using Gafel.Domain.Resources;
using Shouldly;

namespace Validators.Test.Auth.Register;

public class RegisterAccountValidatorTest
{
    [Fact]
    public void Success()
    {
        // Arrange
        var request = RegisterAccountCommandBuilder.Build();
        var validator = new RegisterAccountValidator();

        // Act
        var result = validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Error_FullName_Empty()
    {
        var request = RegisterAccountCommandBuilder.Build();
        request.FullName = string.Empty;
        var validator = new RegisterAccountValidator();

        var result = validator.TestValidate(request);

        result.Errors.ShouldHaveSingleItem();
        result.ShouldHaveValidationErrorFor(x => x.FullName)
            .WithErrorMessage(ResourceMessagesException.PERSON_FULL_NAME_EMPTY);
    }

    [Fact]
    public void Error_Email_Empty()
    {
        var request = RegisterAccountCommandBuilder.Build();
        request.Email = string.Empty;
        var validator = new RegisterAccountValidator();

        var result = validator.TestValidate(request);

        result.Errors.ShouldHaveSingleItem();
        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage(ResourceMessagesException.USER_EMAIL_EMPTY);
    }

    [Fact]
    public void Error_Email_Invalid()
    {
        var request = RegisterAccountCommandBuilder.Build();
        request.Email = "invalid_email";
        var validator = new RegisterAccountValidator();

        var result = validator.TestValidate(request);

        result.Errors.ShouldHaveSingleItem();
        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage(ResourceMessagesException.USER_EMAIL_INVALID);
    }

    [Fact]
    public void Error_Password_Empty()
    {
        var request = RegisterAccountCommandBuilder.Build(1);
        request.Password = string.Empty;
        var validator = new RegisterAccountValidator();

        var result = validator.TestValidate(request);

        result.Errors.ShouldHaveSingleItem();
        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage(ResourceMessagesException.USER_PASSWORD_EMPTY);
    }

    [Theory]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    public void Error_Password_Too_Short(uint passwordLength)
    {
        var request = RegisterAccountCommandBuilder.Build(passwordLength);
        var validator = new RegisterAccountValidator();

        var result = validator.TestValidate(request);

        result.Errors.ShouldHaveSingleItem();
        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage(ResourceMessagesException.USER_PASSWORD_TOO_SHORT);
    }

    [Fact]
    public void Error_Password_Without_Number()
    {
        var request = RegisterAccountCommandBuilder.Build(withNumber: false);
        var validator = new RegisterAccountValidator();

        var result = validator.TestValidate(request);

        result.Errors.ShouldHaveSingleItem();
        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage(ResourceMessagesException.USER_PASSWORD_REQUIRES_NUMBER);
    }

    [Fact]
    public void Error_Password_Without_Special_Character()
    {
        var request = RegisterAccountCommandBuilder.Build(withSpecialCharacter: false);
        var validator = new RegisterAccountValidator();

        var result = validator.TestValidate(request);

        result.Errors.ShouldHaveSingleItem();
        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage(ResourceMessagesException.USER_PASSWORD_REQUIRES_SPECIAL_CHAR);
    }


    [Fact]
    public void Error_Password_Without_Number_And_Without_Special_Character()
    {
        var request = RegisterAccountCommandBuilder.Build(withNumber: false, withSpecialCharacter: false);
        var validator = new RegisterAccountValidator();

        var result = validator.TestValidate(request);

        result.Errors.Count.ShouldBe(2);
        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage(ResourceMessagesException.USER_PASSWORD_REQUIRES_NUMBER);
        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage(ResourceMessagesException.USER_PASSWORD_REQUIRES_SPECIAL_CHAR);
    }
}
