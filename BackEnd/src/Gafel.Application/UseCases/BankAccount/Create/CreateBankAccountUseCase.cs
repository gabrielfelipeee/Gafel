using Gafel.Application.Exceptions;
using Gafel.Application.Extensions;
using Gafel.Application.UseCases.BankAccount.Shared.Commands;
using Gafel.Application.UseCases.BankAccount.Shared.Validators;
using Gafel.Domain.Repositories;
using Gafel.Domain.Repositories.BankAccount;
using Gafel.Domain.Repositories.Person;
using Gafel.Domain.Services.CurrentUser;
using Mapster;

namespace Gafel.Application.UseCases.BankAccount.Create;

public class CreateBankAccountUseCase : ICreateBankAccountUseCase
{
    private readonly ICurrentUser _currentUser;
    private readonly IBankAccountWriteOnlyRepository _bankAccountWriteOnlyRepository;
    private readonly IPersonReadOnlyRepository _personReadOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBankAccountUseCase(
        ICurrentUser currentUser,
        IBankAccountWriteOnlyRepository bankAccountWriteOnlyRepository,
        IPersonReadOnlyRepository personReadOnlyRepository,
        IUnitOfWork unitOfWork)
    {
        _currentUser = currentUser;
        _bankAccountWriteOnlyRepository = bankAccountWriteOnlyRepository;
        _personReadOnlyRepository = personReadOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(BankAccountCommand request)
    {
        Validate(request);

        var currentUser = _currentUser.CurrentUser();
        var person = await _personReadOnlyRepository.GetByUserId(currentUser.Id) ?? throw new PersonNotFoundException();

        var bankAccount = request.Adapt<Domain.Entities.BankAccount>();
        bankAccount.PersonId = person.Id;
        bankAccount.CreatedAt = DateTime.UtcNow;

        await _bankAccountWriteOnlyRepository.Add(bankAccount);
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
