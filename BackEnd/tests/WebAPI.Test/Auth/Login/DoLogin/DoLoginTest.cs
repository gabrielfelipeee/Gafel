using CommonTestUtilities.Commands;
using Gafel.Application.UseCases.Auth.Login.DoLogin;
using Gafel.Domain.Resources;
using Shouldly;
using System.Net;
using System.Text.Json;

namespace WebAPI.Test.Auth.Login.DoLogin;

public class DoLoginTest : GafelClassFixture
{
    private const string METHOD = "auth/login";

    private readonly string _personFullName;
    private readonly string _userEmail;
    private readonly string _userPassword;

    public DoLoginTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _personFullName = factory.GetPersonFullName();
        _userEmail = factory.GetUserEmail();
        _userPassword = factory.GetUserPassword();
    }

    [Fact]
    public async Task Success()
    {
        // Arrange
        var request = new DoLoginCommand()
        {
            Email = _userEmail,
            Password = _userPassword
        };

        // Act
        var response = await DoPost(method: METHOD, request: request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var fullName = responseData.RootElement.GetProperty("fullName").GetString();
        fullName.ShouldBe(_personFullName);

        var tokens = responseData.RootElement.GetProperty("tokens");
        tokens.EnumerateObject().Count().ShouldBe(1);

        var accessToken = tokens.GetProperty("accessToken").GetString();
        accessToken.ShouldNotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Error_Invalid_Credentials()
    {
        var request = DoLoginCommandBuilder.Build();


        var response = await DoPost(method: METHOD, request: request);


        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var title = responseData.RootElement.GetProperty("title").GetString();
        title.ShouldBe(ResourceMessagesException.EXCEPTION_INVALID_LOGIN_TITLE);

        var detail = responseData.RootElement.GetProperty("detail").GetString();
        detail.ShouldBe(ResourceMessagesException.AUTH_INVALID_CREDENTIALS);

        var status = responseData.RootElement.GetProperty("status").GetInt32();
        status.ShouldBe((int)HttpStatusCode.Unauthorized);
    }
}
