using Gafel.Domain.Dtos.QueryParams;
using Gafel.Domain.Entities;
using Gafel.Domain.Repositories.Transaction;
using Microsoft.EntityFrameworkCore;

namespace Gafel.Infrastructure.DataAccess.Repositories;

public class TransactionRepository(GafelDbContext context) : ITransactionReadOnlyRepository, ITransactionWriteOnlyRepository, ITransactionUpdateOnlyRepository
{
    private readonly GafelDbContext _context = context;

    // Read
    async Task<Transaction?> ITransactionReadOnlyRepository.GetById(Person person, Guid transactionId)
        => await _context.Transactions
            .AsNoTracking()
            .Include(x => x.BankAccount)
            .Include(x => x.Category)
            .FirstOrDefaultAsync(transaction => transaction.IsActive && transaction.PersonId == person.Id && transaction.Id == transactionId);

    public async Task<bool> ExistActiveTransactionWithId(Person person, Guid transactionId)
        => await _context.Transactions
            .AsNoTracking()
            .AnyAsync(transaction => transaction.IsActive && transaction.PersonId == person.Id && transaction.Id == transactionId);

    public async Task<(IList<Transaction> transactions, int total)> Filter(Person person, FilterTransactionQueryParams filters)
    {
        IQueryable<Transaction> query = _context.Transactions
            .AsNoTracking()
            .Where(transaction => 
                transaction.IsActive &&
                transaction.PersonId == person.Id &&
                transaction.Date >= filters.StartDate &&
                transaction.Date <= filters.EndDate
            )
            .Include(x => x.BankAccount)
            .Include(x => x.Category)
            .OrderBy(transaction => transaction.Date)
            .ThenBy(transaction => transaction.Id);

        if (filters.Type.HasValue)
            query = query.Where(transaction => transaction.Category.Type == filters.Type.Value);

        if (filters.IsSettled.HasValue)
            query = query.Where(transaction => transaction.IsSettled == filters.IsSettled.Value);

        if (filters.MinimumAmount.HasValue)
            query = query.Where(transaction => transaction.Amount >= filters.MinimumAmount.Value);

        if (filters.MaximumAmount.HasValue)
            query = query.Where(transaction => transaction.Amount <= filters.MaximumAmount.Value);

        if (filters.BankAccountId.HasValue)
            query = query.Where(transaction => transaction.BankAccountId == filters.BankAccountId.Value);

        if (filters.CategoryId.HasValue)
            query = query.Where(transaction => transaction.CategoryId == filters.CategoryId.Value);

        int total = await query.CountAsync();

        query = query.Skip(filters.Offset).Take(filters.Limit);

        return (await query.ToListAsync(), total);
    }


    // Write
    public async Task Add(Transaction transaction) => await _context.Transactions.AddAsync(transaction);

    public async Task Delete(Guid transactionId)
    {
        var transaction = await _context.Transactions.FindAsync(transactionId);
        _context.Transactions.Remove(transaction!);
    }


    // Update
    async Task<Transaction?> ITransactionUpdateOnlyRepository.GetById(Person person, Guid transactionId)
       => await _context.Transactions.FirstOrDefaultAsync(transaction => transaction.IsActive && transaction.PersonId == person.Id && transaction.Id == transactionId);

    public void Update(Transaction transaction) => _context.Transactions.Update(transaction);
}
