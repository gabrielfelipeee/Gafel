using Gafel.Domain.Entities;
using Gafel.Domain.Repositories.Transaction;

namespace Gafel.Infrastructure.DataAccess.Repositories;

public class TransactionRepository(GafelDbContext context) : ITransactionWriteOnlyRepository
{
    private readonly GafelDbContext _context = context;

    // Write
    public async Task Add(Transaction transaction) => await _context.Transactions.AddAsync(transaction);
}
