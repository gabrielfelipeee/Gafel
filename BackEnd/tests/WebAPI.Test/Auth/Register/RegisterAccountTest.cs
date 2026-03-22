using CommonTestUtilities.Commands;
using Gafel.Domain.Resources;
using Shouldly;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace WebAPI.Test.Auth.Register;

public class RegisterAccountTest(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient = factory.CreateClient();

    [Fact]
    public async Task Success()
    {
        // Arrange
        var request = RegisterAccountCommandBuilder.Build();

        // Act
        var response = await _httpClient.PostAsJsonAsync("auth/register", request);

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


        var response = await _httpClient.PostAsJsonAsync("auth/register", request);


        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var title = responseData.RootElement.GetProperty("title").GetString();
        title.ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_TITLE);

        var detail = responseData.RootElement.GetProperty("detail").GetString();
        detail.ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_DETAIL);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateObject().ToList();
        var error = errors.ShouldHaveSingleItem();
        error.Name.ShouldBe("fullName");

        var errorMessages = error.Value.EnumerateArray().ToList();
        var message = errorMessages.ShouldHaveSingleItem().GetString();
        message.ShouldBe(ResourceMessagesException.PERSON_FULL_NAME_EMPTY);
    }
}