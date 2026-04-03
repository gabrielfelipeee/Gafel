using CommonTestUtilities.Commands;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories.Person;
using CommonTestUtilities.Services.Identity;
using CommonTestUtilities.Tokens;
using Gafel.Application.Exceptions;
using Gafel.Application.UseCases.Auth.Login.DoLogin;
using Gafel.Domain.Entities;
using Gafel.Domain.Resources;
using Shouldly;
using System.Net;

namespace UseCases.Test.Auth.Login.DoLogin;

public class DoLoginUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        // Arrange
        var request = DoLoginCommandBuilder.Build();
        var person = PersonBuilder.Build();
        var useCase = CreateUseCase(person);

        // Act
        var result = await useCase.Execute(request);

        // Assert
        result.ShouldNotBeNull();
        result.FullName.ShouldBe(person.FullName);
        result.Tokens.ShouldNotBeNull();
        result.Tokens.AccessToken.ShouldNotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Error_Person_NotFound()
    {
        var request = DoLoginCommandBuilder.Build();
        var useCase = CreateUseCase();

        var exception = await Should.ThrowAsync<PersonNotFoundException>(() => useCase.Execute(request));

        exception.GetErrorTitle().ShouldBe(ResourceMessagesException.EXCEPTION_PERSON_NOT_FOUND_TITLE);
        exception.GetErrorDetail().ShouldBe(ResourceMessagesException.EXCEPTION_PERSON_NOT_FOUND_DETAIL);
        exception.GetStatusCode().ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Error_Invalid_Credentials()
    {
        var request = DoLoginCommandBuilder.Build();
        var useCase = CreateUseCase(invalidCredentials: true);

        var exception = await Should.ThrowAsync<InvalidLoginException>(() => useCase.Execute(request));

        exception.GetErrorTitle().ShouldBe(ResourceMessagesException.EXCEPTION_INVALID_LOGIN_TITLE);
        exception.GetErrorDetail().ShouldBe(ResourceMessagesException.AUTH_INVALID_CREDENTIALS);
        exception.GetStatusCode().ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Invalid_Email()
    {
        var request = DoLoginCommandBuilder.Build();
        request.Email = "invalid_email";
        var useCase = CreateUseCase();

        var exception = await Should.ThrowAsync<ErrorOnValidationException>(() => useCase.Execute(request));

        exception.GetErrorTitle().ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_TITLE);
        exception.GetErrorDetail().ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_DETAIL);
        exception.GetStatusCode().ShouldBe(HttpStatusCode.BadRequest);

        var error = exception.GetErrors().ShouldHaveSingleItem();
        error.Key.ShouldBe(nameof(request.Email));

        var value = error.Value.ShouldHaveSingleItem();
        value.ShouldContain(ResourceMessagesException.USER_EMAIL_INVALID);
    }

    private static DoLoginUseCase CreateUseCase(Gafel.Domain.Entities.Person? person = null, bool invalidCredentials = false)
    {
        var authService = new AuthServiceBuilder();
        if (invalidCredentials)
            authService.LoginWithInvalidCredentials();
        else
            authService.Login();

        var personReadOnlyRepository = new PersonReadOnlyRepositoryBuilder();
        if (person is not null)
            personReadOnlyRepository.GetByUserId(person);

        var accessTokenGenerator = JwtTokenGeneratorBuilder.Build();

        return new DoLoginUseCase(authService.Build(), personReadOnlyRepository.Build(), accessTokenGenerator);
    }
}
