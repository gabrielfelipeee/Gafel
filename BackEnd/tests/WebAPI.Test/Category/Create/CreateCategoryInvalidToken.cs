using CommonTestUtilities.Commands;
using CommonTestUtilities.Tokens;
using Shouldly;
using System.Net;

namespace WebAPI.Test.Category.Create;

public class CreateCategoryInvalidToken(CustomWebApplicationFactory factory) : GafelClassFixture(factory)
{
    private const string METHOD = "categories";

    [Fact]
    public async Task Error_Token_Invalid()
    {
        var request = CategoryCommandBuilder.Build();

        var response = await DoPost(method: METHOD, request: request, token: "invalidToken");

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Without_Token()
    {
        var request = CategoryCommandBuilder.Build();

        var response = await DoPost(method: METHOD, request: request, token: string.Empty);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Token_With_User_NotFound()
    {
        var request = CategoryCommandBuilder.Build();
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: Guid.CreateVersion7());

        var response = await DoPost(method: METHOD, request: request, token: token);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
