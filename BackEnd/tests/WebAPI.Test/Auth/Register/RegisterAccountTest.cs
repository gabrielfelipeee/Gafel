using CommonTestUtilities.Commands;
using Gafel.Domain.Resources;
using Shouldly;
using System.Net;
using System.Text.Json;
using WebAPI.Test.Assertions;

namespace WebAPI.Test.Auth.Register;

public class RegisterAccountTest(CustomWebApplicationFactory factory) : GafelClassFixture(factory)
{
    private const string METHOD = "auth/register";

    [Fact]
    public async Task Success()
    {
        // Arrange
        var request = RegisterAccountCommandBuilder.Build();

        // Act
        var response = await DoPost(method: METHOD, request: request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var fullName = responseData.RootElement.GetProperty("fullName").GetString();
        fullName.ShouldBe(request.FullName);

        var tokens = responseData.RootElement.GetProperty("tokens");
        tokens.EnumerateObject().Count().ShouldBe(1);

        var accessToken = tokens.GetProperty("accessToken").GetString();
        accessToken.ShouldNotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Error_FullName_Empty()
    {
        var request = RegisterAccountCommandBuilder.Build();
        request.FullName = string.Empty;

        var response = await DoPost(method: METHOD, request: request);

        await response.ShouldHaveSingleValidationError(field: "fullName", expectedMessage: ResourceMessagesException.PERSON_FULL_NAME_EMPTY);
    }
}