namespace Gafel.Domain.Repositories.BankAccount;

public interface IBankAccountReadOnlyRepository
{
    Task<Entities.BankAccount?> GetById(Entities.Person person, long bankAccountId);
    Task<bool> ExistActiveBankAccountWithId(Entities.Person person, long bankAccountId);
}
