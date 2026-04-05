using CommonTestUtilities.Commands;
using CommonTestUtilities.Tokens;
using Shouldly;
using System.Net;

namespace WebAPI.Test.Account.UpdateProfile;

public class UpdateProfileInvalidToken(CustomWebApplicationFactory factory) : GafelClassFixture(factory)
{
    private const string METHOD = "me";

    [Fact]
    public async Task Error_Token_Invalid()
    {
        var request = UpdateProfileCommandBuilder.Build();

        var response = await DoPut(method: METHOD, request: request, token: "invalidToken");

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Without_Token()
    {
        var request = UpdateProfileCommandBuilder.Build();

        var response = await DoPut(method: METHOD, request: request, token: string.Empty);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Token_With_User_NotFound()
    {
        var request = UpdateProfileCommandBuilder.Build();
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: 10);

        var response = await DoPut(method: METHOD, request: request, token: token);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
