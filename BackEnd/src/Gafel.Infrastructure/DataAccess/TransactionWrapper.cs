using Gafel.Domain.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Gafel.Infrastructure.DataAccess;

public class TransactionWrapper(IDbContextTransaction transaction) : ITransaction
{
    private readonly IDbContextTransaction _transaction = transaction;

    public async Task CommitAsync() => await _transaction.CommitAsync();
    public async Task RollbackAsync() => await _transaction.RollbackAsync();

    public void Dispose() => _transaction.Dispose();
    public async ValueTask DisposeAsync() => await _transaction.DisposeAsync();
}
