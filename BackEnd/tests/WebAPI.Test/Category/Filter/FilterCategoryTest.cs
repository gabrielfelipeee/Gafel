using CommonTestUtilities.Tokens;
using Gafel.Domain.Dtos;
using Gafel.Domain.Dtos.QueryParams;
using Gafel.Domain.Resources;
using Shouldly;
using System.Net;
using System.Text.Json;
using WebAPI.Test.Assertions;

namespace WebAPI.Test.Category.Filter;

public class FilterCategoryTest : GafelClassFixture
{
    private const string METHOD = "categories";

    private readonly UserDto _user;
    public FilterCategoryTest(CustomWebApplicationFactory factory) : base(factory) => _user = factory.GetUser();


    [Fact]
    public async Task Success()
    {
        // Arrange
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _user.Id);

        // Act -> sem query params (offset/limit)
        var response = await DoGet(method: METHOD, token: token);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        await using var stream = await response.Content.ReadAsStreamAsync();
        var json = await JsonDocument.ParseAsync(stream);

        int total = json.RootElement.GetProperty("total").GetInt32();
        total.ShouldBe(1);

        json.RootElement.GetProperty("offset").GetInt32().ShouldBe(0);
        json.RootElement.GetProperty("limit").GetInt32().ShouldBe(total);
        json.RootElement.GetProperty("items").GetArrayLength().ShouldBe(total);
    }

    [Fact]
    public async Task Error_Limit_Is_Missing_And_Offset_Is_Valid()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _user.Id);
        var url = $"{METHOD}?{nameof(FilterCategoryQueryParams.Offset)}=0";

        var response = await DoGet(method: url, token: token);

        await response.ShouldHaveSingleValidationError(field: "limit", expectedMessage: ResourceMessagesException.PAGINATION_LIMIT_REQUIRED);
    }

    [Fact]
    public async Task Error_Offset_Is_Missing_And_Limit_Is_Valid()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _user.Id);
        var url = $"{METHOD}?{nameof(FilterCategoryQueryParams.Limit)}=10";

        var response = await DoGet(method: url, token: token);

        await response.ShouldHaveSingleValidationError(field: "offset", expectedMessage: ResourceMessagesException.PAGINATION_OFFSET_REQUIRED);
    }

    [Fact]
    public async Task Error_Type_Invalid()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _user.Id);
        var url = $"{METHOD}?{nameof(FilterCategoryQueryParams.Type)}=invalid";

        var response = await DoGet(method: url, token: token);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}
