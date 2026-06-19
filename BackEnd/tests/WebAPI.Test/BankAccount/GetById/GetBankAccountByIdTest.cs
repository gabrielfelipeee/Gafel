using CommonTestUtilities.Tokens;
using Gafel.Domain.Dtos;
using Gafel.Domain.Resources;
using Shouldly;
using System.Net;
using System.Text.Json;

namespace WebAPI.Test.BankAccount.GetById;

public class GetBankAccountByIdTest : GafelClassFixture
{
    private const string METHOD = "bankaccounts";

    private readonly UserDto _user;
    private readonly (Gafel.Domain.Entities.BankAccount entity, string obfuscatedId) _bankAccount;
    public GetBankAccountByIdTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _user = factory.GetUser();
        _bankAccount = factory.GetBankAccount();
    }


    [Fact]
    public async Task Success()
    {
        // Arrange
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _user.Id);

        // Act
        var response = await DoGet(method: $"{METHOD}/{_bankAccount.obfuscatedId}", token: token);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        await using var stream = await response.Content.ReadAsStreamAsync();
        var json = await JsonDocument.ParseAsync(stream);

        json.RootElement.GetProperty("id").GetString().ShouldBe(_bankAccount.obfuscatedId);
        json.RootElement.GetProperty("name").GetString().ShouldBe(_bankAccount.entity.Name);
        json.RootElement.GetProperty("initialBalance").GetDecimal().ShouldBe(_bankAccount.entity.InitialBalance);
        json.RootElement.GetProperty("type").GetInt32().ShouldBe((int)_bankAccount.entity.Type);
    }

    [Fact]
    public async Task Error_BankAccount_NotFound()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _user.Id);

        var response = await DoGet(method: $"{METHOD}/100", token: token);

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        await using var stream = await response.Content.ReadAsStreamAsync();
        var json = await JsonDocument.ParseAsync(stream);

        json.RootElement.GetProperty("title").GetString().ShouldBe(ResourceMessagesException.EXCEPTION_NOT_FOUND_TITLE);
        json.RootElement.GetProperty("detail").GetString().ShouldBe(ResourceMessagesException.BANK_ACCOUNT_NOT_FOUND);
        json.RootElement.GetProperty("status").GetInt32().ShouldBe((int)HttpStatusCode.NotFound);
    }
}
