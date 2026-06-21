namespace Gafel.Application.UseCases.BankAccount.Delete;

public interface IDeleteBankAccountUseCase
{
    Task Execute(long bankAccountId);
}
