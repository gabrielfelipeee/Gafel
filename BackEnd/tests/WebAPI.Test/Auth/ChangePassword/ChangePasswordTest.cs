using CommonTestUtilities.Commands;
using CommonTestUtilities.Tokens;
using Gafel.Application.UseCases.Auth.Login.DoLogin;
using Gafel.Domain.Resources;
using Shouldly;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Gafel.Application.UseCases.Auth.ChangePassword;

namespace WebAPI.Test.Auth.ChangePassword;

public class ChangePasswordTest(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient = factory.CreateClient();

    private readonly string _userEmail = factory.GetUserEmail();
    private readonly string _userPassword = factory.GetUserPassword();
    private readonly long _userId = factory.GetUserId();

    [Fact]
    public async Task Success()
    {
        // Arrange
        var request = ChangePasswordCommandBuilder.Build();
        request.Password = _userPassword;
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _userId);

        // Act
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.PutAsJsonAsync("auth/change-password", request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        var loginRequest = new DoLoginCommand()
        {
            Email = _userEmail,
            Password = _userPassword,
        };

        response = await _httpClient.PostAsJsonAsync("auth/login", loginRequest);
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized); // Senha Antiga

        loginRequest.Password = request.NewPassword; // Senha Atual
        response = await _httpClient.PostAsJsonAsync("auth/login", loginRequest);
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

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.PutAsJsonAsync("auth/change-password", request);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var title = responseData.RootElement.GetProperty("title").GetString();
        title.ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_TITLE);

        var detail = responseData.RootElement.GetProperty("detail").GetString();
        detail.ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_DETAIL);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateObject().ToList();
        var error = errors.ShouldHaveSingleItem();
        error.Name.ShouldBe("newPassword");

        var errorMessages = error.Value.EnumerateArray().ToList();
        var message = errorMessages.ShouldHaveSingleItem().GetString();
        message.ShouldBe(ResourceMessagesException.USER_PASSWORD_EMPTY);
    }
}
