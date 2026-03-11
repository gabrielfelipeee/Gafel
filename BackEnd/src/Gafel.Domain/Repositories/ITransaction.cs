namespace Gafel.Domain.Repositories;

public interface ITransaction : IAsyncDisposable
{
    Task CommitAsync();
    Task RollbackAsync();
}
