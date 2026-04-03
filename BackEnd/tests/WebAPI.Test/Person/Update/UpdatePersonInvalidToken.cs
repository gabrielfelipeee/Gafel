using CommonTestUtilities.Commands;
using CommonTestUtilities.Tokens;
using Shouldly;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace WebAPI.Test.Person.Update;

public class UpdatePersonInvalidToken(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient = factory.CreateClient();

    [Fact]
    public async Task Error_Token_Invalid()
    {
        var request = UpdatePersonCommandBuilder.Build();

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "invalidToken");
        var response = await _httpClient.PutAsJsonAsync("me", request);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Without_Token()
    {
        var request = UpdatePersonCommandBuilder.Build();

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", string.Empty);
        var response = await _httpClient.PutAsJsonAsync("me", request);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Token_With_User_NotFound()
    {
        var request = UpdatePersonCommandBuilder.Build();
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: 10);

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.PutAsJsonAsync("me", request);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
