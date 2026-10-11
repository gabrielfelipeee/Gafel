using CommonTestUtilities.Tokens;
using Gafel.Domain.Dtos;
using Gafel.Domain.Dtos.QueryParams;
using Shouldly;
using System.Net;
using System.Text.Json;

namespace WebAPI.Test.Transaction.Filter;

public class FilterTransactionTest : GafelClassFixture
{
    private const string METHOD = "transactions";

    private readonly UserDto _user;
    private readonly Gafel.Domain.Entities.Transaction _transaction;
    public FilterTransactionTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _user = factory.GetUser();
        _transaction = factory.GetTransaction();
    }

    [Fact]
    public async Task Success()
    {
        // Arrange
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _user.Id);

        // Act
        var response = await DoGet(method: METHOD, token: token);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        await using var stream = await response.Content.ReadAsStreamAsync();
        var json = await JsonDocument.ParseAsync(stream);

        var root = json.RootElement;

        int total = root.GetProperty("total").GetInt32();
        var items = root.GetProperty("items");
        var item = items[0];

        total.ShouldBe(1);
        items.GetArrayLength().ShouldBe(total);

        item.GetProperty("id").GetGuid().ShouldBe(_transaction.Id);
        item.GetProperty("category").GetProperty("id").GetGuid().ShouldBe(_transaction.CategoryId);
        item.GetProperty("bankAccount").GetProperty("id").GetGuid().ShouldBe(_transaction.BankAccountId);
    }

    [Fact]
    public async Task Error_Type_Invalid()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _user.Id);
        var url = $"{METHOD}?{nameof(FilterTransactionQueryParams.Type)}=invalid";

        var response = await DoGet(method: url, token: token);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}
