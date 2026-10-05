namespace Gafel.Application.UseCases.Transaction.Delete;

public interface IDeleteTransactionUseCase
{
    Task Execute(Guid transactionId);
}
