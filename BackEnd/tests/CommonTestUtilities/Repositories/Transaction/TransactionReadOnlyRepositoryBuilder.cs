using Gafel.Domain.Repositories.Transaction;
using Moq;

namespace CommonTestUtilities.Repositories.Transaction;

public class TransactionReadOnlyRepositoryBuilder
{
    private readonly Mock<ITransactionReadOnlyRepository> _repository;
    public TransactionReadOnlyRepositoryBuilder() => _repository = new Mock<ITransactionReadOnlyRepository>();

    public ITransactionReadOnlyRepository Build() => _repository.Object;

    public void GetById(Gafel.Domain.Entities.Person person, Gafel.Domain.Entities.Transaction transaction)
        => _repository.Setup(repository => repository.GetById(person, transaction.Id)).ReturnsAsync(transaction);


    public void ExistActiveTransactionWithId(Gafel.Domain.Entities.Person person, Guid transactionId)
    {
        _repository
            .Setup(repository => repository.ExistActiveTransactionWithId(person, transactionId))
            .ReturnsAsync(true);
    }
}
