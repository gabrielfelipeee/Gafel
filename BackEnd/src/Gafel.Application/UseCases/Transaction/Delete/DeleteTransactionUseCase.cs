using Gafel.Application.Exceptions;
using Gafel.Domain.Repositories;
using Gafel.Domain.Repositories.Person;
using Gafel.Domain.Repositories.Transaction;
using Gafel.Domain.Resources;
using Gafel.Domain.Services.CurrentUser;

namespace Gafel.Application.UseCases.Transaction.Delete;

public class DeleteTransactionUseCase : IDeleteTransactionUseCase
{
    private readonly ICurrentUser _currentUser;
    private readonly IPersonReadOnlyRepository _personReadOnlyRepository;
    private readonly ITransactionReadOnlyRepository _transactionReadOnlyRepository;
    private readonly ITransactionWriteOnlyRepository _transactionWriteOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTransactionUseCase(
        ICurrentUser currentUser,
        IPersonReadOnlyRepository personReadOnlyRepository,
        ITransactionReadOnlyRepository transactionReadOnlyRepository,
        ITransactionWriteOnlyRepository transactionWriteOnlyRepository,
        IUnitOfWork unitOfWork)
    {
        _currentUser = currentUser;
        _personReadOnlyRepository = personReadOnlyRepository;
        _transactionReadOnlyRepository = transactionReadOnlyRepository;
        _transactionWriteOnlyRepository = transactionWriteOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(Guid transactionId)
    {
        var currentUser = _currentUser.CurrentUser();
        var person = await _personReadOnlyRepository.GetByUserId(currentUser.Id) ?? throw new PersonNotFoundException();

        if (!await _transactionReadOnlyRepository.ExistActiveTransactionWithId(person: person, transactionId: transactionId))
            throw new NotFoundException(ResourceMessagesException.TRANSACTION_NOT_FOUND);

        await _transactionWriteOnlyRepository.Delete(transactionId);
        await _unitOfWork.SaveChangesAsync();
    }
}
