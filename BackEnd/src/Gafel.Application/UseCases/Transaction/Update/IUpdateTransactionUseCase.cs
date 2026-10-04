using Gafel.Application.UseCases.Transaction.Shared.Commands;

namespace Gafel.Application.UseCases.Transaction.Update;

public interface IUpdateTransactionUseCase
{
    Task Execute(Guid transactionId, TransactionCommand request);
}
