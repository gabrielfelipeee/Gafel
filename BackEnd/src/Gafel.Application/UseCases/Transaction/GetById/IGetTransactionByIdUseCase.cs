using Gafel.Application.UseCases.Transaction.Shared.Responses;

namespace Gafel.Application.UseCases.Transaction.GetById;

public interface IGetTransactionByIdUseCase
{
    Task<TransactionResponse> Execute(Guid transactionId);
}
