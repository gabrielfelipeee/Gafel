using CommonTestUtilities.Commands;
using FluentValidation.TestHelper;
using Gafel.Application.UseCases.Account.UpdateProfile;
using Gafel.Domain.Resources;
using Shouldly;

namespace Validators.Test.Account.UpdateProfile;

public class UpdateProfileValidatorTest
{
    [Fact]
    public void Success()
    {
        // Arrange
        var request = UpdateProfileCommandBuilder.Build();
        var validator = new UpdateProfileValidator();

        // Act
        var result = validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Error_FullName_Empty()
    {
        var request = UpdateProfileCommandBuilder.Build();
        request.FullName = string.Empty;
        var validator = new UpdateProfileValidator();

        var result = validator.TestValidate(request);

        result.Errors.ShouldHaveSingleItem();
        result.ShouldHaveValidationErrorFor(x => x.FullName)
            .WithErrorMessage(ResourceMessagesException.PERSON_FULL_NAME_EMPTY);
    }

    [Fact]
    public void Error_Uf_Invalid()
    {
        var request = UpdateProfileCommandBuilder.Build();
        request.Uf = "XX";
        var validator = new UpdateProfileValidator();

        var result = validator.TestValidate(request);

        result.Errors.ShouldHaveSingleItem();
        result.ShouldHaveValidationErrorFor(x => x.Uf)
            .WithErrorMessage(ResourceMessagesException.PERSON_UF_INVALID);
    }

    [Fact]
    public void Error_CPF_Length_Invalid()
    {
        var request = UpdateProfileCommandBuilder.Build();
        request.Cpf = "123456789123456";
        var validator = new UpdateProfileValidator();

        var result = validator.TestValidate(request);

        result.Errors.ShouldHaveSingleItem();
        result.ShouldHaveValidationErrorFor(x => x.Cpf)
            .WithErrorMessage(ResourceMessagesException.CPF_INVALID);
    }

    [Fact]
    public void Error_City_Length_Equal_To_2()
    {
        var request = UpdateProfileCommandBuilder.Build();
        request.City = "Vi";
        var validator = new UpdateProfileValidator();

        var result = validator.TestValidate(request);

        result.Errors.ShouldHaveSingleItem();
        result.ShouldHaveValidationErrorFor(x => x.City)
            .WithErrorMessage(ResourceMessagesException.PERSON_CITY_INVALID);
    }
}
