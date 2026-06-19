using Gafel.Domain.Repositories.BankAccount;
using Moq;

namespace CommonTestUtilities.Repositories.BankAccount;

public class BankAccountReadOnlyRepositoryBuilder
{
    private readonly Mock<IBankAccountReadOnlyRepository> _repository;
    public BankAccountReadOnlyRepositoryBuilder() => _repository = new Mock<IBankAccountReadOnlyRepository>();

    public IBankAccountReadOnlyRepository Build() => _repository.Object;

    public void GetById(Gafel.Domain.Entities.Person person, Gafel.Domain.Entities.BankAccount bankAccount)
        => _repository.Setup(repository => repository.GetById(person, bankAccount.Id)).ReturnsAsync(bankAccount);

    public void ExistActiveCategoryWithId(Gafel.Domain.Entities.Person person, long bankAccountId)
    {
        _repository
            .Setup(repository => repository.ExistActiveBankAccountWithId(person, bankAccountId))
            .ReturnsAsync(true);
    }
}
