using CommonTestUtilities.Commands;
using FluentValidation.TestHelper;
using Gafel.Application.UseCases.Person.Update;
using Gafel.Domain.Resources;
using Shouldly;

namespace Validators.Test.Person.Update;

public class UpdatePersonValidatorTest
{
    [Fact]
    public void Success()
    {
        // Arrange
        var request = UpdatePersonCommandBuilder.Build();
        var validator = new UpdatePersonValidator();

        // Act
        var result = validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Error_FullName_Empty()
    {
        var request = UpdatePersonCommandBuilder.Build();
        request.FullName = string.Empty;
        var validator = new UpdatePersonValidator();

        var result = validator.TestValidate(request);

        result.Errors.ShouldHaveSingleItem();
        result.ShouldHaveValidationErrorFor(x => x.FullName)
            .WithErrorMessage(ResourceMessagesException.PERSON_FULL_NAME_EMPTY);
    }

    [Fact]
    public void Error_Uf_Invalid()
    {
        var request = UpdatePersonCommandBuilder.Build();
        request.Uf = "XX";
        var validator = new UpdatePersonValidator();

        var result = validator.TestValidate(request);

        result.Errors.ShouldHaveSingleItem();
        result.ShouldHaveValidationErrorFor(x => x.Uf)
            .WithErrorMessage(ResourceMessagesException.PERSON_UF_INVALID);
    }

    [Fact]
    public void Error_CPF_Length_Invalid()
    {
        var request = UpdatePersonCommandBuilder.Build();
        request.Cpf = "123456789123456";
        var validator = new UpdatePersonValidator();

        var result = validator.TestValidate(request);

        result.Errors.ShouldHaveSingleItem();
        result.ShouldHaveValidationErrorFor(x => x.Cpf)
            .WithErrorMessage(ResourceMessagesException.CPF_INVALID);
    }

    [Fact]
    public void Error_City_Length_Equal_To_2()
    {
        var request = UpdatePersonCommandBuilder.Build();
        request.City = "Vi";
        var validator = new UpdatePersonValidator();

        var result = validator.TestValidate(request);

        result.Errors.ShouldHaveSingleItem();
        result.ShouldHaveValidationErrorFor(x => x.City)
            .WithErrorMessage(ResourceMessagesException.PERSON_CITY_INVALID);
    }
}
