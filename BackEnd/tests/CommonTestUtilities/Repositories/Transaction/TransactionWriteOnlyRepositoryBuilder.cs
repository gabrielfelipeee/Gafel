using Gafel.Domain.Repositories.Transaction;
using Moq;

namespace CommonTestUtilities.Repositories.Transaction;

public static class TransactionWriteOnlyRepositoryBuilder
{
    public static ITransactionWriteOnlyRepository Build()
    {
        var mock = new Mock<ITransactionWriteOnlyRepository>();
        return mock.Object;
    }
}
