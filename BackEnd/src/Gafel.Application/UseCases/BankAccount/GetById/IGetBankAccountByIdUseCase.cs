using Gafel.Application.UseCases.BankAccount.Shared.Responses;

namespace Gafel.Application.UseCases.BankAccount.GetById;

public interface IGetBankAccountByIdUseCase
{
    Task<BankAccountResponse> Execute(long bankAccountId);
}
