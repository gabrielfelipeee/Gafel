namespace Gafel.Domain.Repositories;

public interface IUnitOfWork
{
    Task ExecuteInTransactionAsync(Func<Task> action);
    Task SaveChangesAsync();
}
