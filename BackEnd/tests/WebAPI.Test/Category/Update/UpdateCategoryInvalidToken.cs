using CommonTestUtilities.Commands;
using CommonTestUtilities.Tokens;
using Shouldly;
using System.Net;

namespace WebAPI.Test.Category.Update;

public class UpdateCategoryInvalidToken : GafelClassFixture
{
    private const string METHOD = "categories";

    private readonly (Gafel.Domain.Entities.Category entity, string obfuscatedId) _category;
    public UpdateCategoryInvalidToken(CustomWebApplicationFactory factory) : base(factory) => _category = factory.GetCategory();

    [Fact]
    public async Task Error_Token_Invalid()
    {
        var request = CategoryCommandBuilder.Build();

        var response = await DoPut(method: $"{METHOD}/{_category.obfuscatedId}", request: request, token: "invalidToken");

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Without_Token()
    {
        var request = CategoryCommandBuilder.Build();

        var response = await DoPut(method: $"{METHOD}/{_category.obfuscatedId}", request: request, token: string.Empty);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Token_With_User_NotFound()
    {
        var request = CategoryCommandBuilder.Build();
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: 10);

        var response = await DoPut(method: $"{METHOD}/{_category.obfuscatedId}", request: request, token: token);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
