using CommonTestUtilities.Tokens;
using Shouldly;
using System.Net;

namespace WebAPI.Test.Transaction.GetById;

public class GetTransactionByIdInvalidToken : GafelClassFixture
{
    private const string METHOD = "transactions";

    private readonly Gafel.Domain.Entities.Transaction _transaction;
    public GetTransactionByIdInvalidToken(CustomWebApplicationFactory factory) : base(factory) => _transaction = factory.GetTransaction();

    [Fact]
    public async Task Error_Token_Invalid()
    {
        var response = await DoGet(method: $"{METHOD}/{_transaction.Id}", token: "invalidToken");

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Without_Token()
    {
        var response = await DoGet(method: $"{METHOD}/{_transaction.Id}", token: string.Empty);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Token_With_User_NotFound()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: Guid.CreateVersion7());

        var response = await DoGet(method: $"{METHOD}/{_transaction.Id}", token: token);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
