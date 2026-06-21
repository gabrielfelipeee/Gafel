using Gafel.Application.Exceptions;
using Gafel.Domain.Repositories;
using Gafel.Domain.Repositories.BankAccount;
using Gafel.Domain.Repositories.Person;
using Gafel.Domain.Resources;
using Gafel.Domain.Services.CurrentUser;

namespace Gafel.Application.UseCases.BankAccount.Delete;

public class DeleteBankAccountUseCase : IDeleteBankAccountUseCase
{
    private readonly ICurrentUser _currentUser;
    private readonly IPersonReadOnlyRepository _personReadOnlyRepository;
    private readonly IBankAccountReadOnlyRepository _categoryReadOnlyRepository;
    private readonly IBankAccountWriteOnlyRepository _categoryWriteOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBankAccountUseCase(
        ICurrentUser currentUser,
        IPersonReadOnlyRepository personReadOnlyRepository,
        IBankAccountReadOnlyRepository bankAccountReadOnlyRepository,
        IBankAccountWriteOnlyRepository bankAccountWriteOnlyRepository,
        IUnitOfWork unitOfWork)
    {
        _currentUser = currentUser;
        _personReadOnlyRepository = personReadOnlyRepository;
        _categoryReadOnlyRepository = bankAccountReadOnlyRepository;
        _categoryWriteOnlyRepository = bankAccountWriteOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(long bankAccountId)
    {
        var currentUser = _currentUser.CurrentUser();
        var person = await _personReadOnlyRepository.GetByUserId(currentUser.Id) ?? throw new PersonNotFoundException();

        if (!await _categoryReadOnlyRepository.ExistActiveBankAccountWithId(person: person, bankAccountId: bankAccountId))
            throw new NotFoundException(ResourceMessagesException.BANK_ACCOUNT_NOT_FOUND);

        await _categoryWriteOnlyRepository.Delete(bankAccountId);
        await _unitOfWork.SaveChangesAsync();
    }
}
