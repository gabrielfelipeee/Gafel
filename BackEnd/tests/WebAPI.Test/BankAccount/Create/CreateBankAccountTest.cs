using CommonTestUtilities.Commands;
using CommonTestUtilities.Tokens;
using Gafel.Domain.Dtos;
using Gafel.Domain.Enums;
using Gafel.Domain.Resources;
using Shouldly;
using System.Net;
using WebAPI.Test.Assertions;

namespace WebAPI.Test.BankAccount.Create;

public class CreateBankAccountTest : GafelClassFixture
{
    private const string METHOD = "bankaccounts";

    private readonly UserDto _user;

    public CreateBankAccountTest(CustomWebApplicationFactory factory) : base(factory) => _user = factory.GetUser();


    [Fact]
    public async Task Success()
    {
        // Arrange
        var request = BankAccountCommandBuilder.Build();
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _user.Id);

        // Act
        var response = await DoPost(method: METHOD, request: request, token: token);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var request = BankAccountCommandBuilder.Build();
        request.Name = string.Empty;
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _user.Id);

        var response = await DoPost(method: METHOD, request: request, token: token);

        await response.ShouldHaveSingleValidationError(field: "name", expectedMessage: ResourceMessagesException.NAME_REQUIRED);
    }

    [Fact]
    public async Task Error_InitialBalance_Negative()
    {
        var request = BankAccountCommandBuilder.Build();
        request.InitialBalance = -100;
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _user.Id);

        var response = await DoPost(method: METHOD, request: request, token: token);

        await response.ShouldHaveSingleValidationError(field: "initialBalance", expectedMessage: ResourceMessagesException.BANK_ACCOUNT_INITIAL_BALANCE_OUT_OF_RANGE);
    }

    [Fact]
    public async Task Error_Type_Invalid()
    {
        var request = BankAccountCommandBuilder.Build();
        request.Type = (BankAccountType)100;
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _user.Id);

        var response = await DoPost(method: METHOD, request: request, token: token);

        await response.ShouldHaveSingleValidationError(field: "type", expectedMessage: ResourceMessagesException.TYPE_INVALID);
    }
}
