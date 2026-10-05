using Gafel.Domain.Repositories.Transaction;
using Moq;

namespace CommonTestUtilities.Repositories.Transaction;

public class TransactionUpdateOnlyRepositoryBuilder
{
    private readonly Mock<ITransactionUpdateOnlyRepository> _repository;
    public TransactionUpdateOnlyRepositoryBuilder() => _repository = new Mock<ITransactionUpdateOnlyRepository>();

    public ITransactionUpdateOnlyRepository Build() => _repository.Object;

    public void GetById(Gafel.Domain.Entities.Person person, Gafel.Domain.Entities.Transaction transaction)
        => _repository.Setup(repository => repository.GetById(person, transaction.Id)).ReturnsAsync(transaction);
}
