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
            .FirstOrDefaultAsync(transaction => transaction.IsActive && transaction.PersonId == person.Id && transaction.Id == transactionId);

    public async Task<bool> ExistActiveTransactionWithId(Person person, Guid transactionId)
        => await _context.Transactions
            .AsNoTracking()
            .AnyAsync(transaction => transaction.IsActive && transaction.PersonId == person.Id && transaction.Id == transactionId);


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
