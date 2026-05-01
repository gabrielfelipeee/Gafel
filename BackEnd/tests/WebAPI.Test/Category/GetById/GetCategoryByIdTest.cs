using CommonTestUtilities.Tokens;
using Gafel.Domain.Dtos;
using Gafel.Domain.Resources;
using Shouldly;
using System.Net;
using System.Text.Json;

namespace WebAPI.Test.Category.GetById;

public class GetCategoryByIdTest : GafelClassFixture
{
    private const string METHOD = "categories";

    private readonly UserDto _user;
    private readonly Gafel.Domain.Entities.Category _category;
    public GetCategoryByIdTest(CustomWebApplicationFactory factory) : base(factory)
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
        var response = await DoGet(method: $"{METHOD}/{_category.Id}", token: token);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        await using var stream = await response.Content.ReadAsStreamAsync();
        var json = await JsonDocument.ParseAsync(stream);

        json.RootElement.GetProperty("id").GetInt64().ShouldBe(_category.Id);
        json.RootElement.GetProperty("name").GetString().ShouldBe(_category.Name);
        json.RootElement.GetProperty("icon").GetString().ShouldBe(_category.Icon);
        json.RootElement.GetProperty("type").GetInt32().ShouldBe((int)_category.Type);
    }

    [Fact]
    public async Task Error_Category_NotFound()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _user.Id);

        var response = await DoGet(method: $"{METHOD}/100", token: token);

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        await using var stream = await response.Content.ReadAsStreamAsync();
        var json = await JsonDocument.ParseAsync(stream);

        json.RootElement.GetProperty("title").GetString().ShouldBe(ResourceMessagesException.EXCEPTION_NOT_FOUND_TITLE);
        json.RootElement.GetProperty("detail").GetString().ShouldBe(ResourceMessagesException.CATEGORY_NOT_FOUND);
        json.RootElement.GetProperty("status").GetInt32().ShouldBe((int)HttpStatusCode.NotFound);
    }
}
