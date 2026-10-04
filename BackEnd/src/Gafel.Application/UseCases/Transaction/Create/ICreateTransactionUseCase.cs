using Gafel.Application.UseCases.Transaction.Shared.Commands;

namespace Gafel.Application.UseCases.Transaction.Create;

public interface ICreateTransactionUseCase
{
    Task Execute(TransactionCommand request);
}
