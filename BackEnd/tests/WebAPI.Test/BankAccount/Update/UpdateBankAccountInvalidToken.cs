using CommonTestUtilities.Commands;
using CommonTestUtilities.Tokens;
using Shouldly;
using System.Net;

namespace WebAPI.Test.BankAccount.Update;

public class UpdateBankAccountInvalidToken : GafelClassFixture
{
    private const string METHOD = "bankaccounts";

    private readonly (Gafel.Domain.Entities.BankAccount entity, string obfuscatedId) _bankAccount;
    public UpdateBankAccountInvalidToken(CustomWebApplicationFactory factory) : base(factory) => _bankAccount = factory.GetBankAccount();

    [Fact]
    public async Task Error_Token_Invalid()
    {
        var request = BankAccountCommandBuilder.Build();

        var response = await DoPut(method: $"{METHOD}/{_bankAccount.obfuscatedId}", request: request, token: "invalidToken");

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Without_Token()
    {
        var request = BankAccountCommandBuilder.Build();

        var response = await DoPut(method: $"{METHOD}/{_bankAccount.obfuscatedId}", request: request, token: string.Empty);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Token_With_User_NotFound()
    {
        var request = BankAccountCommandBuilder.Build();
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: 10);

        var response = await DoPut(method: $"{METHOD}/{_bankAccount.obfuscatedId}", request: request, token: token);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
