using Gafel.Domain.Repositories;
using Moq;

namespace CommonTestUtilities.Repositories;

public static class UnitOfWorkBuilder
{
    public static IUnitOfWork Build()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        unitOfWorkMock
            .Setup(u => u.ExecuteInTransactionAsync(It.IsAny<Func<Task>>()))
            .Returns<Func<Task>>(async action => await action());

        unitOfWorkMock
            .Setup(u => u.SaveChangesAsync())
            .Returns(Task.CompletedTask);

        return unitOfWorkMock.Object;
    }
}
