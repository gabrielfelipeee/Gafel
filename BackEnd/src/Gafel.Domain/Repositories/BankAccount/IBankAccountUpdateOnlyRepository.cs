namespace Gafel.Domain.Repositories.BankAccount;

public interface IBankAccountUpdateOnlyRepository
{
    Task<Entities.BankAccount?> GetById(Entities.Person person, Guid bankAccountId);
    void Update(Entities.BankAccount bankAccount);
}
