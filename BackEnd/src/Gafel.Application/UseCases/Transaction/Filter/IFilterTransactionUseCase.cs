using Gafel.Application.UseCases.Shared.Responses;
using Gafel.Application.UseCases.Transaction.Shared.Responses;
using Gafel.Domain.Dtos.QueryParams;

namespace Gafel.Application.UseCases.Transaction.Filter;

public interface IFilterTransactionUseCase
{
    Task<PaginationResponse<TransactionResponse>> Execute(FilterTransactionQueryParams query);
}
