using Gafel.Application.UseCases.BankAccount.Shared.Commands;

namespace Gafel.Application.UseCases.BankAccount.Update;

public interface IUpdateBankAccountUseCase
{
    Task Execute(long bankAccountId, BankAccountCommand request);
}
