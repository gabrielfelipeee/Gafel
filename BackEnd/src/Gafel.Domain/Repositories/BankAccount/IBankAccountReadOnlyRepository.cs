using Gafel.Domain.Dtos.QueryParams;

namespace Gafel.Domain.Repositories.BankAccount;

public interface IBankAccountReadOnlyRepository
{
    Task<Entities.BankAccount?> GetById(Entities.Person person, long bankAccountId);
    Task<(IList<Entities.BankAccount> bankAccounts, int total)> Filter(Entities.Person person, FilterBankAccountQueryParams filters);
    Task<bool> ExistActiveBankAccountWithId(Entities.Person person, long bankAccountId);
}
