using Gafel.Domain.Repositories.BankAccount;
using Moq;

namespace CommonTestUtilities.Repositories.BankAccount;

public class BankAccountUpdateOnlyRepositoryBuilder
{
    private readonly Mock<IBankAccountUpdateOnlyRepository> _repository;
    public BankAccountUpdateOnlyRepositoryBuilder() => _repository = new Mock<IBankAccountUpdateOnlyRepository>();

    public IBankAccountUpdateOnlyRepository Build() => _repository.Object;

    public void GetById(Gafel.Domain.Entities.Person person, Gafel.Domain.Entities.BankAccount bankAccount)
        => _repository.Setup(repository => repository.GetById(person, bankAccount.Id)).ReturnsAsync(bankAccount);
}
