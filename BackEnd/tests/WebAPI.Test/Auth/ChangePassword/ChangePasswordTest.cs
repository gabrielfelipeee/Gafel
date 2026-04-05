using CommonTestUtilities.Commands;
using CommonTestUtilities.Tokens;
using Gafel.Application.UseCases.Auth.Login.DoLogin;
using Gafel.Domain.Resources;
using Shouldly;
using System.Net;
using Gafel.Application.UseCases.Auth.ChangePassword;
using WebAPI.Test.Assertions;

namespace WebAPI.Test.Auth.ChangePassword;

public class ChangePasswordTest : GafelClassFixture
{
    private const string METHOD = "auth/change-password";

    private readonly string _userEmail;
    private readonly string _userPassword;
    private readonly long _userId;
    public ChangePasswordTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _userEmail = factory.GetUserEmail();
        _userPassword = factory.GetUserPassword();
        _userId = factory.GetUserId();
    }

    [Fact]
    public async Task Success()
    {
        // Arrange
        var request = ChangePasswordCommandBuilder.Build();
        request.Password = _userPassword;
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _userId);

        // Act
        var response = await DoPut(method: METHOD, request: request, token: token);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        var loginRequest = new DoLoginCommand()
        {
            Email = _userEmail,
            Password = _userPassword,
        };

        response = await DoPost(method: "auth/login", request: loginRequest);
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized); // Senha Antiga

        loginRequest.Password = request.NewPassword; // Senha Atual
        response = await DoPost(method: "auth/login", request: loginRequest);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Error_NewPassword_Empty()
    {
        var request = new ChangePasswordCommand
        {
            Password = _userPassword,
            NewPassword = string.Empty
        };
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _userId);

        var response = await DoPut(method: METHOD, request: request, token: token);

        await response.ShouldHaveSingleValidationError(field: "newPassword", expectedMessage: ResourceMessagesException.USER_PASSWORD_EMPTY);
    }
}
