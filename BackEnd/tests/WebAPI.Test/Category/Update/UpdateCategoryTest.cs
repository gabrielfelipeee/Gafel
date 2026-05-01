using CommonTestUtilities.Commands;
using CommonTestUtilities.Tokens;
using Gafel.Domain.Dtos;
using Gafel.Domain.Enums;
using Gafel.Domain.Resources;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebAPI.Test.Assertions;

namespace WebAPI.Test.Category.Update;

public class UpdateCategoryTest : GafelClassFixture
{
    private const string METHOD = "categories";

    private readonly UserDto _user;
    private readonly Gafel.Domain.Entities.Category _category;
    public UpdateCategoryTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _user = factory.GetUser();
        _category = factory.GetCategory();
    }


    [Fact]
    public async Task Success()
    {
        // Arrange
        var request = CategoryCommandBuilder.Build();
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _user.Id);

        // Act
        var response = await DoPut(method: $"{METHOD}/{_category.Id}", request: request, token: token);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Error_Category_NotFound()
    {
        var request = CategoryCommandBuilder.Build();
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _user.Id);

        var response = await DoPut(method: $"{METHOD}/100", request: request, token: token);

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        await using var stream = await response.Content.ReadAsStreamAsync();
        var json = await JsonDocument.ParseAsync(stream);

        json.RootElement.GetProperty("title").GetString()
            .ShouldBe(ResourceMessagesException.EXCEPTION_NOT_FOUND_TITLE);

        json.RootElement.GetProperty("detail").GetString()
            .ShouldBe(ResourceMessagesException.CATEGORY_NOT_FOUND);

        json.RootElement.GetProperty("status").GetInt32()
            .ShouldBe((int)HttpStatusCode.NotFound);
    }


    [Fact]
    public async Task Error_Name_Empty()
    {
        var request = CategoryCommandBuilder.Build();
        request.Name = string.Empty;
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _user.Id);

        var response = await DoPut(method: $"{METHOD}/{_category.Id}", request: request, token: token);

        await response.ShouldHaveSingleValidationError(field: "name", expectedMessage: ResourceMessagesException.CATEGORY_NAME_EMPTY);
    }

    [Fact]
    public async Task Error_Icon_Empty()
    {
        var request = CategoryCommandBuilder.Build();
        request.Icon = string.Empty;
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _user.Id);

        var response = await DoPut(method: $"{METHOD}/{_category.Id}", request: request, token: token);

        await response.ShouldHaveSingleValidationError(field: "icon", expectedMessage: ResourceMessagesException.CATEGORY_ICON_EMPTY);
    }

    [Fact]
    public async Task Error_Type_Invalid()
    {
        var request = CategoryCommandBuilder.Build();
        request.Type = (CategoryType)100;
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _user.Id);

        var response = await DoPut(method: $"{METHOD}/{_category.Id}", request: request, token: token);

        await response.ShouldHaveSingleValidationError(field: "type", expectedMessage: ResourceMessagesException.CATEGORY_TYPE_INVALID);
    }
}
