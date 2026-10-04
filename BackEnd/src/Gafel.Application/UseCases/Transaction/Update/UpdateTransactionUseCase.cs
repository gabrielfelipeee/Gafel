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

namespace Gafel.Application.UseCases.Transaction.Update;

public class UpdateTransactionUseCase : IUpdateTransactionUseCase
{
    private readonly ICurrentUser _currentUser;
    private readonly IPersonReadOnlyRepository _personReadOnlyRepository;
    private readonly ITransactionUpdateOnlyRepository _transactionUpdateOnlyRepository;
    private readonly IBankAccountReadOnlyRepository _bankAccountReadOnlyRepository;
    private readonly ICategoryReadOnlyRepository _categoryReadOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTransactionUseCase(
        ICurrentUser currentUser,
        IPersonReadOnlyRepository personReadOnlyRepository,
        ITransactionUpdateOnlyRepository transactionUpdateOnlyRepository,
        IBankAccountReadOnlyRepository bankAccountReadOnlyRepository,
        ICategoryReadOnlyRepository categoryReadOnlyRepository,
        IUnitOfWork unitOfWork)
    {
        _currentUser = currentUser;
        _personReadOnlyRepository = personReadOnlyRepository;
        _transactionUpdateOnlyRepository = transactionUpdateOnlyRepository;
        _bankAccountReadOnlyRepository = bankAccountReadOnlyRepository;
        _categoryReadOnlyRepository = categoryReadOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(Guid transactionId, TransactionCommand request)
    {
        Validate(request);

        var currentUser = _currentUser.CurrentUser();
        var person = await _personReadOnlyRepository.GetByUserId(currentUser.Id) ?? throw new PersonNotFoundException();

        if (!await _bankAccountReadOnlyRepository.ExistActiveBankAccountWithId(person, request.BankAccountId))
            throw new NotFoundException(ResourceMessagesException.BANK_ACCOUNT_NOT_FOUND);

        if (!await _categoryReadOnlyRepository.ExistActiveCategoryWithId(person, request.CategoryId))
            throw new NotFoundException(ResourceMessagesException.CATEGORY_NOT_FOUND);


        var transaction = await _transactionUpdateOnlyRepository.GetById(person, transactionId) ?? throw new NotFoundException(ResourceMessagesException.TRANSACTION_NOT_FOUND);
        request.Adapt(transaction);

        _transactionUpdateOnlyRepository.Update(transaction);
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
