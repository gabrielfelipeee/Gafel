using CommonTestUtilities.Tokens;
using Gafel.Domain.Dtos;
using Gafel.Domain.Resources;
using Microsoft.AspNetCore.Http;
using Shouldly;
using System.Net;
using System.Text.Json;

namespace WebAPI.Test.Transaction.GetById;

public class GetTransactionByIdTest : GafelClassFixture
{
    private const string METHOD = "transactions";

    private readonly UserDto _user;
    private readonly Gafel.Domain.Entities.Transaction _transaction;
    public GetTransactionByIdTest(CustomWebApplicationFactory factory) : base(factory)
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
        var response = await DoGet(method: $"{METHOD}/{_transaction.Id}", token: token);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        await using var stream = await response.Content.ReadAsStreamAsync();
        var json = await JsonDocument.ParseAsync(stream);

        var root = json.RootElement;

        root.GetProperty("id").GetGuid().ShouldBe(_transaction.Id);
        root.GetProperty("description").GetString().ShouldBe(_transaction.Description);
        DateOnly.Parse(root.GetProperty("date").GetString()!).ShouldBe(_transaction.Date);
        root.GetProperty("amount").GetDecimal().ShouldBe(_transaction.Amount);
        root.GetProperty("isSettled").GetBoolean().ShouldBe(_transaction.IsSettled);
        root.GetProperty("category").GetProperty("id").GetGuid().ShouldBe(_transaction.CategoryId);
        root.GetProperty("bankAccount").GetProperty("id").GetGuid().ShouldBe(_transaction.BankAccountId);
    }

    [Fact]
    public async Task Error_Transaction_NotFound()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _user.Id);

        var response = await DoGet(method: $"{METHOD}/{Guid.CreateVersion7()}", token: token);

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        await using var stream = await response.Content.ReadAsStreamAsync();
        var json = await JsonDocument.ParseAsync(stream);

        json.RootElement.GetProperty("title").GetString().ShouldBe(ResourceMessagesException.EXCEPTION_NOT_FOUND_TITLE);
        json.RootElement.GetProperty("detail").GetString().ShouldBe(ResourceMessagesException.TRANSACTION_NOT_FOUND);
        json.RootElement.GetProperty("status").GetInt32().ShouldBe(StatusCodes.Status404NotFound);
    }
}
