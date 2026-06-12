using CommonTestUtilities.Tokens;
using Shouldly;
using System.Net;

namespace WebAPI.Test.Category.Delete;

public class DeleteCategoryInvalidToken : GafelClassFixture
{
    private const string METHOD = "categories";

    private readonly (Gafel.Domain.Entities.Category entity, string obfuscatedId) _category;
    public DeleteCategoryInvalidToken(CustomWebApplicationFactory factory) : base(factory) => _category = factory.GetCategory();

    [Fact]
    public async Task Error_Token_Invalid()
    {
        var response = await DoGet(method: $"{METHOD}/{_category.obfuscatedId}", token: "invalidToken");

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Without_Token()
    {
        var response = await DoGet(method: $"{METHOD}/{_category.obfuscatedId}", token: string.Empty);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Token_With_User_NotFound()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: 10);

        var response = await DoGet(method: $"{METHOD}/{_category.obfuscatedId}", token: token);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
