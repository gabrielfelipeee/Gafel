using CommonTestUtilities.Tokens;
using Gafel.Domain.Dtos;
using Gafel.Domain.Entities;
using Shouldly;
using System.Net;
using System.Text.Json;

namespace WebAPI.Test.Account.GetProfile;

public class GetProfileTest : GafelClassFixture
{
    private const string METHOD = "me";

    private readonly UserDto _user;
    private readonly Person _person;
    public GetProfileTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _user = factory.GetUser();
        _person = factory.GetPerson();
    }

    [Fact]
    public async Task Success()
    {
        // Arrange
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _user.Id);

        // Act
        var response = await DoGet(method: METHOD, token: token);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("email").GetString().ShouldBe(_user.Email);
        responseData.RootElement.GetProperty("fullName").GetString().ShouldBe(_person.FullName);
        responseData.RootElement.GetProperty("cpf").GetString().ShouldBe(_person.Cpf!.Value);
    }
}
