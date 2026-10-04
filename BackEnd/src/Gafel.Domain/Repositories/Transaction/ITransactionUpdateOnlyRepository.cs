namespace Gafel.Domain.Repositories.Transaction;

public interface ITransactionUpdateOnlyRepository
{
    Task<Entities.Transaction?> GetById(Entities.Person person, Guid transactionId);
    void Update(Entities.Transaction transaction);
}
