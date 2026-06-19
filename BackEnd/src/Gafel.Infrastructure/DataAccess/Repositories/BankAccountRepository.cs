using Gafel.Domain.Entities;
using Gafel.Domain.Repositories.BankAccount;
using Microsoft.EntityFrameworkCore;

namespace Gafel.Infrastructure.DataAccess.Repositories;

public class BankAccountRepository(GafelDbContext context) : IBankAccountReadOnlyRepository, IBankAccountWriteOnlyRepository
{
    private readonly GafelDbContext _context = context;

    // Read
    public async Task<bool> ExistActiveBankAccountWithId(Person person, long bankAccountId)
        => await _context.BankAccounts
        .AsNoTracking()
        .AnyAsync(bankAccount => bankAccount.IsActive && bankAccount.PersonId == person.Id && bankAccount.Id == bankAccountId);

    public async Task<BankAccount?> GetById(Person person, long bankAccountId) => await _context.BankAccounts
        .AsNoTracking()
        .FirstOrDefaultAsync(bankAccount => bankAccount.IsActive && bankAccount.PersonId == person.Id && bankAccount.Id == bankAccountId);

    // Write
    public async Task Add(BankAccount bankAccount) => await _context.BankAccounts.AddAsync(bankAccount);

    public async Task Delete(long bankAccountId)
    {
        var bankAccount = await _context.BankAccounts.FindAsync(bankAccountId);
        _context.BankAccounts.Remove(bankAccount!);
    }
}
