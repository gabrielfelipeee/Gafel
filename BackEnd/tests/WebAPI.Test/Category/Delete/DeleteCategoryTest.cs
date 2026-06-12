using CommonTestUtilities.Tokens;
using Gafel.Domain.Dtos;
using Gafel.Domain.Resources;
using Shouldly;
using System.Net;
using System.Text.Json;

namespace WebAPI.Test.Category.Delete;

public class DeleteCategoryTest : GafelClassFixture
{
    private const string METHOD = "categories";

    private readonly UserDto _user;
    private readonly (Gafel.Domain.Entities.Category entity, string obfuscatedId) _category;
    public DeleteCategoryTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _user = factory.GetUser();
        _category = factory.GetCategory();
    }

    [Fact]
    public async Task Success()
    {
        // Arrange
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _user.Id);

        // Act
        var response = await DoDelete(method: $"{METHOD}/{_category.obfuscatedId}", token: token);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        response = await DoGet($"{METHOD}/{_category.obfuscatedId}", token: token);
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Error_Category_NotFound()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _user.Id);

        var response = await DoDelete(method: $"{METHOD}/100", token: token);

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        await using var stream = await response.Content.ReadAsStreamAsync();
        var json = await JsonDocument.ParseAsync(stream);

        json.RootElement.GetProperty("title").GetString().ShouldBe(ResourceMessagesException.EXCEPTION_NOT_FOUND_TITLE);
        json.RootElement.GetProperty("detail").GetString().ShouldBe(ResourceMessagesException.CATEGORY_NOT_FOUND);
        json.RootElement.GetProperty("status").GetInt32().ShouldBe((int)HttpStatusCode.NotFound);

    }
}
