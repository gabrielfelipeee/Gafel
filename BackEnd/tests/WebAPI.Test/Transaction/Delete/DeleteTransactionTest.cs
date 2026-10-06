using CommonTestUtilities.Tokens;
using Gafel.Domain.Dtos;
using Gafel.Domain.Resources;
using Shouldly;
using System.Net;
using System.Text.Json;

namespace WebAPI.Test.Transaction.Delete;

public class DeleteTransactionTest : GafelClassFixture
{
    private const string METHOD = "transactions";

    private readonly UserDto _user;
    private readonly Gafel.Domain.Entities.Transaction _transaction;
    public DeleteTransactionTest(CustomWebApplicationFactory factory) : base(factory)
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
        var response = await DoDelete(method: $"{METHOD}/{_transaction.Id}", token: token);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Error_Transaction_NotFound()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _user.Id);

        var response = await DoDelete(method: $"{METHOD}/{Guid.CreateVersion7()}", token: token);

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        await using var stream = await response.Content.ReadAsStreamAsync();
        var json = await JsonDocument.ParseAsync(stream);

        json.RootElement.GetProperty("title").GetString().ShouldBe(ResourceMessagesException.EXCEPTION_NOT_FOUND_TITLE);
        json.RootElement.GetProperty("detail").GetString().ShouldBe(ResourceMessagesException.TRANSACTION_NOT_FOUND);
        json.RootElement.GetProperty("status").GetInt32().ShouldBe((int)HttpStatusCode.NotFound);
    }
}
