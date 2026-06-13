using Gafel.Domain.Repositories.BankAccount;
using Moq;

namespace CommonTestUtilities.Repositories.BankAccount;

public static class BankAccountWriteOnlyRepositoryBuilder
{
    public static IBankAccountWriteOnlyRepository Build()
    {
        var mock = new Mock<IBankAccountWriteOnlyRepository>();
        return mock.Object;
    }
}
