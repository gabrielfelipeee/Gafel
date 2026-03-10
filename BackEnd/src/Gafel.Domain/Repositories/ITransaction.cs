namespace Gafel.Domain.Repositories;

public interface ITransaction
{
    Task CommitAsync();
    Task RollbackAsync();
}
