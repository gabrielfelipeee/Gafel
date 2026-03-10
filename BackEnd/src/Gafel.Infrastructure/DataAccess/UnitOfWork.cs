using Gafel.Domain.Repositories;

namespace Gafel.Infrastructure.DataAccess;

public class UnitOfWork(GafelDbContext dbContext) : IUnitOfWork
{
    private readonly GafelDbContext _dbContext = dbContext;

    public async Task SaveChangesAsync() => await _dbContext.SaveChangesAsync();

    public async Task<ITransaction> BeginTransactionAsync()
    {
        var transaction = await _dbContext.Database.BeginTransactionAsync();
        return new TransactionWrapper(transaction); // Envolve a transação real no seu Wrapper
    }
}


