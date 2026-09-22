using Gafel.Application.UseCases.BankAccount.Shared.Commands;

namespace Gafel.Application.UseCases.BankAccount.Update;

public interface IUpdateBankAccountUseCase
{
    Task Execute(Guid bankAccountId, BankAccountCommand request);
}
