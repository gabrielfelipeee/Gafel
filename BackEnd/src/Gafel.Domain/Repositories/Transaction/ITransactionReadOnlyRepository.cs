using Gafel.Domain.Dtos.QueryParams;

namespace Gafel.Domain.Repositories.Transaction;

public interface ITransactionReadOnlyRepository
{
    Task<Entities.Transaction?> GetById(Entities.Person person, Guid transactionId);
    Task<bool> ExistActiveTransactionWithId(Entities.Person person, Guid transactionId);
    Task<(IList<Entities.Transaction> transactions, int total)> Filter(Entities.Person person, FilterTransactionQueryParams filters);
}
