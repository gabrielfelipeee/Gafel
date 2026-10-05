namespace Gafel.Domain.Repositories.Transaction;

public interface ITransactionReadOnlyRepository
{
    Task<Entities.Transaction?> GetById(Entities.Person person, Guid transactionId);
    Task<bool> ExistActiveTransactionWithId(Entities.Person person, Guid transactionId);
}
