using CommonTestUtilities.Commands;
using CommonTestUtilities.Tokens;
using Gafel.Domain.Dtos;
using Gafel.Domain.Enums;
using Gafel.Domain.Resources;
using Shouldly;
using System.Net;
using System.Text.Json;
using WebAPI.Test.Assertions;

namespace WebAPI.Test.BankAccount.Update;

public class UpdateBankAccountTest : GafelClassFixture
{
    private const string METHOD = "bankaccounts";

    private readonly UserDto _user;
    private readonly (Gafel.Domain.Entities.BankAccount entity, string obfuscatedId) _bankAccount;
    public UpdateBankAccountTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _user = factory.GetUser();
        _bankAccount = factory.GetBankAccount();
    }


    [Fact]
    public async Task Success()
    {
        // Arrange
        var request = BankAccountCommandBuilder.Build();
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _user.Id);

        // Act
        var response = await DoPut(method: $"{METHOD}/{_bankAccount.obfuscatedId}", request: request, token: token);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Error_BankAccount_NotFound()
    {
        var request = BankAccountCommandBuilder.Build();
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _user.Id);

        var response = await DoPut(method: $"{METHOD}/100", request: request, token: token);

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        await using var stream = await response.Content.ReadAsStreamAsync();
        var json = await JsonDocument.ParseAsync(stream);

        json.RootElement.GetProperty("title").GetString()
            .ShouldBe(ResourceMessagesException.EXCEPTION_NOT_FOUND_TITLE);

        json.RootElement.GetProperty("detail").GetString()
            .ShouldBe(ResourceMessagesException.BANK_ACCOUNT_NOT_FOUND);

        json.RootElement.GetProperty("status").GetInt32()
            .ShouldBe((int)HttpStatusCode.NotFound);
    }


    [Fact]
    public async Task Error_Name_Empty()
    {
        var request = BankAccountCommandBuilder.Build();
        request.Name = string.Empty;
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _user.Id);

        var response = await DoPut(method: $"{METHOD}/{_bankAccount.obfuscatedId}", request: request, token: token);

        await response.ShouldHaveSingleValidationError(field: "name", expectedMessage: ResourceMessagesException.BANK_ACCOUNT_NAME_EMPTY);
    }

    [Fact]
    public async Task Error_InitialBalance_Negative()
    {
        var request = BankAccountCommandBuilder.Build();
        request.InitialBalance = -100;
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _user.Id);

        var response = await DoPut(method: $"{METHOD}/{_bankAccount.obfuscatedId}", request: request, token: token);

        await response.ShouldHaveSingleValidationError(field: "initialBalance", expectedMessage: ResourceMessagesException.INVALID_INITIAL_BALANCE);
    }

    [Fact]
    public async Task Error_Type_Invalid()
    {
        var request = BankAccountCommandBuilder.Build();
        request.Type = (BankAccountType)100;
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _user.Id);

        var response = await DoPut(method: $"{METHOD}/{_bankAccount.obfuscatedId}", request: request, token: token);

        await response.ShouldHaveSingleValidationError(field: "type", expectedMessage: ResourceMessagesException.BANK_ACCOUNT_TYPE_INVALID);
    }
}
