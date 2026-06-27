using Gafel.Application.Exceptions;
using Gafel.Application.Extensions;
using Gafel.Application.UseCases.BankAccount.Shared.Commands;
using Gafel.Application.UseCases.BankAccount.Shared.Validators;
using Gafel.Domain.Repositories;
using Gafel.Domain.Repositories.BankAccount;
using Gafel.Domain.Repositories.Person;
using Gafel.Domain.Resources;
using Gafel.Domain.Services.CurrentUser;
using Mapster;

namespace Gafel.Application.UseCases.BankAccount.Update;

public class UpdateBankAccountUseCase : IUpdateBankAccountUseCase
{
    private readonly ICurrentUser _currentUser;
    private readonly IBankAccountUpdateOnlyRepository _bankAccountUpdateOnlyRepository;
    private readonly IPersonReadOnlyRepository _personReadOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateBankAccountUseCase(
        ICurrentUser currentUser,
        IBankAccountUpdateOnlyRepository bankAccountUpdateOnlyRepository,
        IPersonReadOnlyRepository personReadOnlyRepository,
        IUnitOfWork unitOfWork)
    {
        _currentUser = currentUser;
        _bankAccountUpdateOnlyRepository = bankAccountUpdateOnlyRepository;
        _personReadOnlyRepository = personReadOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(long bankAccountId, BankAccountCommand request)
    {
        Validate(request);

        var currentUser = _currentUser.CurrentUser();
        var person = await _personReadOnlyRepository.GetByUserId(currentUser.Id) ?? throw new PersonNotFoundException();

        var bankAccount = await _bankAccountUpdateOnlyRepository.GetById(person, bankAccountId) ?? throw new NotFoundException(ResourceMessagesException.BANK_ACCOUNT_NOT_FOUND);
        request.Adapt(bankAccount);
        bankAccount.UpdatedAt = DateTime.UtcNow;

        _bankAccountUpdateOnlyRepository.Update(bankAccount);
        await _unitOfWork.SaveChangesAsync();
    }

    private static void Validate(BankAccountCommand request)
    {
        var result = new BankAccountValidator().Validate(request);

        if (!result.IsValid)
        {
            var errors = result.Errors.ToErrorsByPropertyDictionary();

            throw new ErrorOnValidationException(errors);
        }
    }
}
