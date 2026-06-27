using Gafel.Domain.Dtos.QueryParams;
using Gafel.Domain.Entities;
using Gafel.Domain.Enums;
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

    public async Task<(IList<BankAccount> bankAccounts, int total)> Filter(Person person, FilterBankAccountQueryParams filters)
    {
        IQueryable<BankAccount> query = _context.BankAccounts
            .AsNoTracking()
            .Where(bankAccount => bankAccount.IsActive && bankAccount.PersonId == person.Id)
            .OrderBy(x => x.Id);

        if (filters.Type.HasValue)
            query = query.Where(bankAccount => bankAccount.Type == filters.Type.Value);

        if (!string.IsNullOrWhiteSpace(filters.BankAccountName))
            query = query.Where(bankAccount => EF.Functions.Collate(bankAccount.Name, "SQL_Latin1_General_CP1_CI_AI").Contains(filters.BankAccountName));

        int total = await query.CountAsync();

        if (filters.Offset.HasValue && filters.Limit.HasValue)
        {
            query = query
                .Skip(filters.Offset.Value)
                .Take(filters.Limit.Value);
        }

        return (await query.ToListAsync(), total);
    }


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
