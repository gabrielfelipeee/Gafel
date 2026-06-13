namespace Gafel.Domain.Repositories.BankAccount;

public interface IBankAccountWriteOnlyRepository
{
    Task Add(Entities.BankAccount bankAccount);
    Task Delete(long bankAccountId);
}
