using CommonTestUtilities.Commands;
using CommonTestUtilities.Tokens;
using Gafel.Application.UseCases.Auth.ChangePassword;
using Gafel.Application.UseCases.Auth.Login.DoLogin;
using Gafel.Domain.Dtos;
using Gafel.Domain.Resources;
using Shouldly;
using System.Net;
using WebAPI.Test.Assertions;

namespace WebAPI.Test.Auth.ChangePassword;

public class ChangePasswordTest : GafelClassFixture
{
    private const string METHOD = "auth/change-password";

    private readonly UserDto _user;
    private readonly string _userPassword;

    public ChangePasswordTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _user = factory.GetUser();
        _userPassword = factory.GetUserPassword();
    }

    [Fact]
    public async Task Success()
    {
        // Arrange
        var request = ChangePasswordCommandBuilder.Build();
        request.Password = _userPassword;
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _user.Id);

        // Act
        var response = await DoPut(method: METHOD, request: request, token: token);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        var loginRequest = new DoLoginCommand()
        {
            Email = _user.Email,
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
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _user.Id);

        var response = await DoPut(method: METHOD, request: request, token: token);

        await response.ShouldHaveSingleValidationError(field: "newPassword", expectedMessage: ResourceMessagesException.USER_PASSWORD_EMPTY);
    }
}
