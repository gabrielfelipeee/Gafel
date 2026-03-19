using CommonTestUtilities.Commands;
using CommonTestUtilities.Services.Identity;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Person;
using CommonTestUtilities.Tokens;
using Gafel.Application.Exceptions;
using Gafel.Application.UseCases.Auth.Register;
using Gafel.Domain.Resources;
using Shouldly;

namespace UseCases.Test.Auth.Register;

public class RegisterAccountUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        // Arrange
        var request = RegisterAccountCommandBuilder.Build();
        var useCase = CreateUseCase();

        // Act
        var result = await useCase.Execute(request);

        // Assert
        result.ShouldNotBeNull();
        result.FullName.ShouldBe(request.FullName);
        result.Tokens.ShouldNotBeNull();
        result.Tokens.AccessToken.ShouldNotBeNullOrWhiteSpace();
    }


    [Fact]
    public async Task Error_Email_Already_Registered()
    {
        var request = RegisterAccountCommandBuilder.Build();
        var useCase = CreateUseCase(emailAlreadyInUse: true);

        var exception = await Should.ThrowAsync<ErrorOnValidationException>(async () => await useCase.Execute(request));

        exception.GetErrorTitle().ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_TITLE);
        exception.GetErrorDetail().ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_DETAIL);

        var error = exception.GetErrors().ShouldHaveSingleItem();
        error.Key.ShouldBe(nameof(request.Email));

        var value = error.Value.ShouldHaveSingleItem();
        value.ShouldContain(ResourceMessagesException.USER_EMAIL_ALREADY_REGISTERED);
    }

    [Fact]
    public async Task Error_FullName_Empty()
    {
        var request = RegisterAccountCommandBuilder.Build();
        request.FullName = string.Empty;
        var useCase = CreateUseCase();

        var exception = await Should.ThrowAsync<ErrorOnValidationException>(async () => await useCase.Execute(request));

        exception.GetErrorTitle().ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_TITLE);
        exception.GetErrorDetail().ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_DETAIL);

        var error = exception.GetErrors().ShouldHaveSingleItem();
        error.Key.ShouldBe(nameof(request.FullName));

        var value = error.Value.ShouldHaveSingleItem();
        value.ShouldContain(ResourceMessagesException.PERSON_FULL_NAME_EMPTY);
    }


    private static RegisterAccountUseCase CreateUseCase(bool emailAlreadyInUse = false)
    {
        var personWriteOnlyRepository = PersonWriteOnlyRepositoryBuilder.Build();

        var userWriteOnlyService = new UserWriteOnlyServiceBuilder();
        if (emailAlreadyInUse)
            userWriteOnlyService.RegisterWithEmailAlreadyInUse();
        else
            userWriteOnlyService.Register();

        var accessTokenGenerator = JwtTokenGeneratorBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();

        return new(
            personWriteOnlyRepository,
            userWriteOnlyService.Build(),
            accessTokenGenerator,
            unitOfWork);
    }
}
