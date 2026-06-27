using CommonTestUtilities.Tokens;
using Shouldly;
using System.Net;

namespace WebAPI.Test.BankAccount.Filter;

public class FilterBankAccountInvalidToken : GafelClassFixture
{
    private const string METHOD = "bankaccounts";
    public FilterBankAccountInvalidToken(CustomWebApplicationFactory factory) : base(factory) { }

    [Fact]
    public async Task Error_Token_Invalid()
    {
        var response = await DoGet(method: METHOD, token: "invalidToken");

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Without_Token()
    {
        var response = await DoGet(method: METHOD, token: string.Empty);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Token_With_User_NotFound()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: 10);

        var response = await DoGet(method: METHOD, token: token);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
