using CommonTestUtilities.Tokens;
using Shouldly;
using System.Net;
using System.Text.Json;

namespace WebAPI.Test.Account.GetProfile;

public class GetProfileTest : GafelClassFixture
{
    private const string METHOD = "me";

    private readonly long _userId;
    private readonly string _userEmail;
    private readonly string _personFullName;
    private readonly string _personCpf;
    public GetProfileTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _userId = factory.GetUserId();
        _userEmail = factory.GetUserEmail();
        _personFullName = factory.GetPersonFullName();
        _personCpf = factory.GetPersonCpf();
    }

    [Fact]
    public async Task Success()
    {
        // Arrange
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _userId);

        // Act
        var response = await DoGet(method: METHOD, token: token);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("email").GetString().ShouldBe(_userEmail);
        responseData.RootElement.GetProperty("fullName").GetString().ShouldBe(_personFullName);
        responseData.RootElement.GetProperty("cpf").GetString().ShouldBe(_personCpf);
    }
}
