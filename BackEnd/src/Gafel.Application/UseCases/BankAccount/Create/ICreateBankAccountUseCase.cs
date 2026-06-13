using Gafel.Application.UseCases.BankAccount.Shared.Commands;

namespace Gafel.Application.UseCases.BankAccount.Create;

public interface ICreateBankAccountUseCase
{
    Task Execute(BankAccountCommand request);
}
