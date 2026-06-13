using Gafel.Domain.Entities;
using Gafel.Domain.Repositories.BankAccount;

namespace Gafel.Infrastructure.DataAccess.Repositories;

public class BankAccountRepository(GafelDbContext context) : IBankAccountWriteOnlyRepository
{
    private readonly GafelDbContext _context = context;

    // Write
    public async Task Add(BankAccount bankAccount) => await _context.BankAccounts.AddAsync(bankAccount);

    public async Task Delete(long bankAccountId)
    {
        var bankAccount = await _context.BankAccounts.FindAsync(bankAccountId);
        _context.BankAccounts.Remove(bankAccount!);
    }
}
