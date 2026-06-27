using CommonTestUtilities.Tokens;
using Shouldly;
using System.Net;

namespace WebAPI.Test.BankAccount.Delete;

public class DeleteBankAccountInvalidToken : GafelClassFixture
{
    private const string METHOD = "bankaccounts";

    private readonly (Gafel.Domain.Entities.BankAccount entity, string obfuscatedId) _bankAccount;
    public DeleteBankAccountInvalidToken(CustomWebApplicationFactory factory) : base(factory) => _bankAccount = factory.GetBankAccount();

    [Fact]
    public async Task Error_Token_Invalid()
    {
        var response = await DoGet(method: $"{METHOD}/{_bankAccount.obfuscatedId}", token: "invalidToken");

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Without_Token()
    {
        var response = await DoGet(method: $"{METHOD}/{_bankAccount.obfuscatedId}", token: string.Empty);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Token_With_User_NotFound()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: 10);

        var response = await DoGet(method: $"{METHOD}/{_bankAccount.obfuscatedId}", token: token);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
