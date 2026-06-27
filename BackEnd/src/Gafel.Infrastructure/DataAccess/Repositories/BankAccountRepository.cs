using Gafel.Domain.Entities;
using Gafel.Domain.Repositories.BankAccount;
using Microsoft.EntityFrameworkCore;

namespace Gafel.Infrastructure.DataAccess.Repositories;

public class BankAccountRepository(GafelDbContext context) : IBankAccountReadOnlyRepository, IBankAccountWriteOnlyRepository, IBankAccountUpdateOnlyRepository
{
    private readonly GafelDbContext _context = context;

    // Read
    async Task<BankAccount?> IBankAccountReadOnlyRepository.GetById(Person person, long bankAccountId) => await _context.BankAccounts
        .AsNoTracking()
        .FirstOrDefaultAsync(bankAccount => bankAccount.IsActive && bankAccount.PersonId == person.Id && bankAccount.Id == bankAccountId);

    public async Task<bool> ExistActiveBankAccountWithId(Person person, long bankAccountId)
        => await _context.BankAccounts
        .AsNoTracking()
        .AnyAsync(bankAccount => bankAccount.IsActive && bankAccount.PersonId == person.Id && bankAccount.Id == bankAccountId);


    // Write
    public async Task Add(BankAccount bankAccount) => await _context.BankAccounts.AddAsync(bankAccount);

    public async Task Delete(long bankAccountId)
    {
        var bankAccount = await _context.BankAccounts.FindAsync(bankAccountId);
        _context.BankAccounts.Remove(bankAccount!);
    }


    // Update
    async Task<BankAccount?> IBankAccountUpdateOnlyRepository.GetById(Person person, long bankAccountId)
        => await _context.BankAccounts.FirstOrDefaultAsync(bankAccount => bankAccount.IsActive && bankAccount.PersonId == person.Id && bankAccount.Id == bankAccountId);

    public void Update(BankAccount bankAccount) => _context.BankAccounts.Update(bankAccount);
}
