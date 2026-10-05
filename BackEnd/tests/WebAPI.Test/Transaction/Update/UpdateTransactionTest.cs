using CommonTestUtilities.Commands;
using CommonTestUtilities.Tokens;
using Gafel.Domain.Dtos;
using Gafel.Domain.Resources;
using Shouldly;
using System.Net;
using System.Text.Json;
using WebAPI.Test.Assertions;

namespace WebAPI.Test.Transaction.Update;

public class UpdateTransactionTest : GafelClassFixture
{
    private const string METHOD = "transactions";

    private readonly UserDto _user;
    private readonly Gafel.Domain.Entities.Transaction _transaction;
    private readonly Guid _bankAccountId;
    private readonly Guid _categoryId;

    public UpdateTransactionTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _user = factory.GetUser();
        _transaction = factory.GetTransaction();
        _bankAccountId = factory.GetBankAccount().Id;
        _categoryId = factory.GetCategory().Id;
    }


    [Fact]
    public async Task Success()
    {
        // Arrange
        var request = TransactionCommandBuilder.Build(categoryId: _categoryId, bankAccountId: _bankAccountId);
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _user.Id);

        // Act
        var response = await DoPut(method: $"{METHOD}/{_transaction.Id}", request: request, token: token);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Error_Transaction_NotFound()
    {
        var request = TransactionCommandBuilder.Build(categoryId: _categoryId, bankAccountId: _bankAccountId);
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _user.Id);

        var response = await DoPut(method: $"{METHOD}/{Guid.CreateVersion7()}", request: request, token: token);

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        await using var stream = await response.Content.ReadAsStreamAsync();
        var json = await JsonDocument.ParseAsync(stream);
        json.RootElement.GetProperty("title").GetString().ShouldBe(ResourceMessagesException.EXCEPTION_NOT_FOUND_TITLE);
        json.RootElement.GetProperty("detail").GetString().ShouldBe(ResourceMessagesException.TRANSACTION_NOT_FOUND);
        json.RootElement.GetProperty("status").GetInt32().ShouldBe((int)HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Error_BankAccount_NotFound()
    {
        var request = TransactionCommandBuilder.Build(categoryId: _categoryId);
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _user.Id);

        var response = await DoPut(method: $"{METHOD}/{_transaction.Id}", request: request, token: token);

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        await using var stream = await response.Content.ReadAsStreamAsync();
        var json = await JsonDocument.ParseAsync(stream);
        json.RootElement.GetProperty("title").GetString().ShouldBe(ResourceMessagesException.EXCEPTION_NOT_FOUND_TITLE);
        json.RootElement.GetProperty("detail").GetString().ShouldBe(ResourceMessagesException.BANK_ACCOUNT_NOT_FOUND);
        json.RootElement.GetProperty("status").GetInt32().ShouldBe((int)HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Error_Category_NotFound()
    {
        var request = TransactionCommandBuilder.Build(bankAccountId: _bankAccountId);
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _user.Id);

        var response = await DoPut(method: $"{METHOD}/{_transaction.Id}", request: request, token: token);

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        await using var stream = await response.Content.ReadAsStreamAsync();
        var json = await JsonDocument.ParseAsync(stream);
        json.RootElement.GetProperty("title").GetString().ShouldBe(ResourceMessagesException.EXCEPTION_NOT_FOUND_TITLE);
        json.RootElement.GetProperty("detail").GetString().ShouldBe(ResourceMessagesException.CATEGORY_NOT_FOUND);
        json.RootElement.GetProperty("status").GetInt32().ShouldBe((int)HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Error_Amount_Zero()
    {
        var request = TransactionCommandBuilder.Build();
        request.Amount = 0m;
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _user.Id);

        var response = await DoPut(method: $"{METHOD}/{_transaction.Id}", request: request, token: token);

        await response.ShouldHaveSingleValidationError(field: "amount", expectedMessage: ResourceMessagesException.TRANSACTION_AMOUNT_OUT_OF_RANGE);
    }
}
