using CommonTestUtilities.Commands;
using CommonTestUtilities.Services.CurrentUser;
using CommonTestUtilities.Services.Identity;
using Gafel.Application.Exceptions;
using Gafel.Application.UseCases.Auth.ChangePassword;
using Gafel.Domain.Resources;
using Shouldly;

namespace UseCases.Test.Auth.ChangePassword;

public class ChangePasswordUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var request = ChangePasswordCommandBuilder.Build();
        var useCase = CreateUseCase();

        async Task act() => await useCase.Execute(request);

        await Should.NotThrowAsync(act);
    }

    [Fact]
    public async Task Error_NewPassword_Empty()
    {
        var request = ChangePasswordCommandBuilder.Build();
        request.NewPassword = string.Empty;
        var useCase = CreateUseCase();

        var exception = await Should.ThrowAsync<ErrorOnValidationException>(() => useCase.Execute(request));

        exception.GetErrorTitle().ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_TITLE);
        exception.GetErrorDetail().ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_DETAIL);

        var error = exception.GetErrors().ShouldHaveSingleItem();
        error.Key.ShouldBe(nameof(request.NewPassword));

        var value = error.Value.ShouldHaveSingleItem();
        value.ShouldContain(ResourceMessagesException.USER_PASSWORD_EMPTY);
    }

    [Fact]
    public async Task Error_NewPassword_Invalid()
    {
        var request = ChangePasswordCommandBuilder.Build();
        request.NewPassword = "123";
        var useCase = CreateUseCase();

        var exception = await Should.ThrowAsync<ErrorOnValidationException>(() => useCase.Execute(request));

        exception.GetErrorTitle().ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_TITLE);
        exception.GetErrorDetail().ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_DETAIL);

        var error = exception.GetErrors().ShouldHaveSingleItem();
        error.Key.ShouldBe(nameof(request.NewPassword));

        var value = error.Value;
        value.Length.ShouldBe(2);
        value.ShouldContain(ResourceMessagesException.USER_PASSWORD_TOO_SHORT);
        value.ShouldContain(ResourceMessagesException.USER_PASSWORD_REQUIRES_SPECIAL_CHAR);
    }


    [Fact]
    public async Task Error_CurrentPassword_Different()
    {
        var request = ChangePasswordCommandBuilder.Build();
        var useCase = CreateUseCase(currentPasswordDifferent: true);

        var exception = await Should.ThrowAsync<ErrorOnValidationException>(() => useCase.Execute(request));

        exception.GetErrorTitle().ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_TITLE);
        exception.GetErrorDetail().ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_DETAIL);

        var error = exception.GetErrors().ShouldHaveSingleItem();
        error.Key.ShouldBe(nameof(request.Password));

        var value = error.Value.ShouldHaveSingleItem();
        value.ShouldContain(ResourceMessagesException.USER_PASSWORD_INCORRECT);
    }

    private static ChangePasswordUseCase CreateUseCase(bool currentPasswordDifferent = false)
    {
        var authService = new AuthServiceBuilder();
        if (currentPasswordDifferent)
            authService.ChangePasswordWithCurrentPasswordDifferent();
        else
            authService.ChangePassword();

        var currentUser = CurrentUserBuilder.Build();

        return new(authService.Build(), currentUser);
    }
}
