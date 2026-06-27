using Gafel.Application.UseCases.BankAccount.Shared.Responses;
using Gafel.Application.UseCases.Shared.Responses;
using Gafel.Domain.Dtos.QueryParams;

namespace Gafel.Application.UseCases.BankAccount.Filter;

public interface IFilterBankAccountUseCase
{
    Task<PaginationResponse<BankAccountResponse>> Execute(FilterBankAccountQueryParams query);
}
