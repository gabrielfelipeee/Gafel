using Gafel.Application.Exceptions;
using Gafel.Application.Extensions;
using Gafel.Application.UseCases.Transaction.Shared.Commands;
using Gafel.Application.UseCases.Transaction.Shared.Validators;
using Gafel.Domain.Repositories;
using Gafel.Domain.Repositories.BankAccount;
using Gafel.Domain.Repositories.Category;
using Gafel.Domain.Repositories.Person;
using Gafel.Domain.Repositories.Transaction;
using Gafel.Domain.Resources;
using Gafel.Domain.Services.CurrentUser;
using Mapster;

namespace Gafel.Application.UseCases.Transaction.Create;

public class CreateTransactionUseCase : ICreateTransactionUseCase
{
    private readonly ICurrentUser _currentUser;
    private readonly ITransactionWriteOnlyRepository _transactionWriteOnlyRepository;
    private readonly IBankAccountReadOnlyRepository _bankAccountReadOnlyRepository;
    private readonly ICategoryReadOnlyRepository _categoryReadOnlyRepository;
    private readonly IPersonReadOnlyRepository _personReadOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTransactionUseCase(
        ICurrentUser currentUser,
        IPersonReadOnlyRepository personReadOnlyRepository,
        ITransactionWriteOnlyRepository transactionWriteOnlyRepository,
        ICategoryReadOnlyRepository categoryReadOnlyRepository,
        IBankAccountReadOnlyRepository bankAccountReadOnlyRepository,
        IUnitOfWork unitOfWork)
    {
        _currentUser = currentUser;
        _personReadOnlyRepository = personReadOnlyRepository;
        _transactionWriteOnlyRepository = transactionWriteOnlyRepository;
        _bankAccountReadOnlyRepository = bankAccountReadOnlyRepository;
        _categoryReadOnlyRepository = categoryReadOnlyRepository;
        _unitOfWork = unitOfWork;
    }


    public async Task Execute(TransactionCommand request)
    {
        Validate(request);

        var currentUser = _currentUser.CurrentUser();
        var person = await _personReadOnlyRepository.GetByUserId(currentUser.Id) ?? throw new PersonNotFoundException();

        if (!await _bankAccountReadOnlyRepository.ExistActiveBankAccountWithId(person, request.BankAccountId))
            throw new NotFoundException(ResourceMessagesException.BANK_ACCOUNT_NOT_FOUND);

        if (!await _categoryReadOnlyRepository.ExistActiveCategoryWithId(person, request.CategoryId))
            throw new NotFoundException(ResourceMessagesException.CATEGORY_NOT_FOUND);


        var transaction = request.Adapt<Domain.Entities.Transaction>();
        transaction.PersonId = person.Id;

        await _transactionWriteOnlyRepository.Add(transaction);
        await _unitOfWork.SaveChangesAsync();
    }

    private static void Validate(TransactionCommand request)
    {
        var result = new TransactionValidator().Validate(request);

        if (!result.IsValid)
        {
            var errors = result.Errors.ToErrorsByPropertyDictionary();

            throw new ErrorOnValidationException(errors);
        }
    }
}
