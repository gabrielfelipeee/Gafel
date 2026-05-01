using Gafel.Domain.Repositories.Category;
using Moq;

namespace CommonTestUtilities.Repositories.Category;

public static class CategoryWriteOnlyRepositoryBuilder
{
    public static ICategoryWriteOnlyRepository Build()
    {
        var mock = new Mock<ICategoryWriteOnlyRepository>();
        return mock.Object;
    }
}
