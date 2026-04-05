using CommonTestUtilities.Commands;
using CommonTestUtilities.Tokens;
using Shouldly;
using System.Net;

namespace WebAPI.Test.Auth.ChangePassword;

public class ChangePasswordInvalidTokenTest(CustomWebApplicationFactory factory) : GafelClassFixture(factory)
{
    private const string METHOD = "auth/change-password";

    [Fact]
    public async Task Error_Token_Invalid()
    {
        var request = ChangePasswordCommandBuilder.Build();

        var response = await DoPut(method: METHOD, request: request, token: "invalidToken");

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Without_Token()
    {
        var request = ChangePasswordCommandBuilder.Build();

        var response = await DoPut(method: METHOD, request: request, token: string.Empty);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Token_With_User_NotFound()
    {
        var request = ChangePasswordCommandBuilder.Build();
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: 10);

        var response = await DoPut(method: METHOD, request: request, token: token);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
