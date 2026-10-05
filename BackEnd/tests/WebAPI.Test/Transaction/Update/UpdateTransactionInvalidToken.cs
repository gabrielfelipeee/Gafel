using CommonTestUtilities.Commands;
using CommonTestUtilities.Tokens;
using Shouldly;
using System.Net;

namespace WebAPI.Test.Transaction.Update;

public class UpdateTransactionInvalidToken : GafelClassFixture
{
    private const string METHOD = "transactions";

    private readonly Gafel.Domain.Entities.Transaction _transaction;
    public UpdateTransactionInvalidToken(CustomWebApplicationFactory factory) : base(factory) => _transaction = factory.GetTransaction();

    [Fact]
    public async Task Error_Token_Invalid()
    {
        var request = TransactionCommandBuilder.Build();

        var response = await DoPut(method: $"{METHOD}/{_transaction.Id}", request: request, token: "invalidToken");

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Without_Token()
    {
        var request = TransactionCommandBuilder.Build();

        var response = await DoPut(method: $"{METHOD}/{_transaction.Id}", request: request, token: string.Empty);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Token_With_User_NotFound()
    {
        var request = TransactionCommandBuilder.Build();
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: Guid.CreateVersion7());

        var response = await DoPut(method: $"{METHOD}/{_transaction.Id}", request: request, token: token);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
