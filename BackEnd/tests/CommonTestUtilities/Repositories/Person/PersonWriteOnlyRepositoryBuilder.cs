using Gafel.Domain.Repositories.Person;
using Moq;

namespace CommonTestUtilities.Repositories.Person;

public static class PersonWriteOnlyRepositoryBuilder
{
    public static IPersonWriteOnlyRepository Build()
    {
        var mock = new Mock<IPersonWriteOnlyRepository>();
        return mock.Object;
    }
}
