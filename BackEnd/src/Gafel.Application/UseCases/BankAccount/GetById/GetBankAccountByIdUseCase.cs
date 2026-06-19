using Gafel.Application.Exceptions;
using Gafel.Application.UseCases.BankAccount.Shared.Responses;
using Gafel.Domain.Repositories.BankAccount;
using Gafel.Domain.Repositories.Person;
using Gafel.Domain.Resources;
using Gafel.Domain.Services.CurrentUser;
using Mapster;

namespace Gafel.Application.UseCases.BankAccount.GetById;

public class GetBankAccountByIdUseCase : IGetBankAccountByIdUseCase
{
    private readonly ICurrentUser _currentUser;
    private readonly IPersonReadOnlyRepository _personReadOnlyRepository;
    private readonly IBankAccountReadOnlyRepository _bankAccountReadOnlyRepository;

    public GetBankAccountByIdUseCase(
        ICurrentUser currentUser,
        IPersonReadOnlyRepository personReadOnlyRepository,
        IBankAccountReadOnlyRepository bankAccountReadOnlyRepository
        )
    {
        _currentUser = currentUser;
        _personReadOnlyRepository = personReadOnlyRepository;
        _bankAccountReadOnlyRepository = bankAccountReadOnlyRepository;
    }

    public async Task<BankAccountResponse> Execute(long bankAccountId)
    {
        var currentUser = _currentUser.CurrentUser();
        var person = await _personReadOnlyRepository.GetByUserId(currentUser.Id) ?? throw new PersonNotFoundException();

        var bankAccount = await _bankAccountReadOnlyRepository.GetById(person: person, bankAccountId: bankAccountId)
            ?? throw new NotFoundException(ResourceMessagesException.BANK_ACCOUNT_NOT_FOUND);

        var response = bankAccount.Adapt<BankAccountResponse>();

        return response;
    }
}
