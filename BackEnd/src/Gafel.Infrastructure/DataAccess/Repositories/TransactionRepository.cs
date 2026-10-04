using Gafel.Domain.Entities;
using Gafel.Domain.Repositories.Transaction;
using Microsoft.EntityFrameworkCore;

namespace Gafel.Infrastructure.DataAccess.Repositories;

public class TransactionRepository(GafelDbContext context) : ITransactionWriteOnlyRepository, ITransactionUpdateOnlyRepository
{
    private readonly GafelDbContext _context = context;

    // Write
    public async Task Add(Transaction transaction) => await _context.Transactions.AddAsync(transaction);

    // Update
    public async Task<Transaction?> GetById(Person person, Guid transactionId)
        => await _context.Transactions.FirstOrDefaultAsync(transaction => transaction.IsActive && transaction.PersonId == person.Id && transaction.Id == transactionId);

    public void Update(Transaction transaction) => _context.Transactions.Update(transaction);
}
