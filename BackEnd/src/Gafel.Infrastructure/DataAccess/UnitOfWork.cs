using Gafel.Domain.Repositories;

namespace Gafel.Infrastructure.DataAccess;

public class UnitOfWork(GafelDbContext dbContext) : IUnitOfWork
{
    private readonly GafelDbContext _context = dbContext;

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

    public async Task ExecuteInTransactionAsync(Func<Task> action)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            await action();
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}


