using Gafel.Application.Exceptions;
using Gafel.Application.UseCases.Transaction.Shared.Responses;
using Gafel.Domain.Repositories.Person;
using Gafel.Domain.Repositories.Transaction;
using Gafel.Domain.Resources;
using Gafel.Domain.Services.CurrentUser;
using Mapster;

namespace Gafel.Application.UseCases.Transaction.GetById;

public class GetTransactionByIdUseCase : IGetTransactionByIdUseCase
{
    private readonly ICurrentUser _currentUser;
    private readonly IPersonReadOnlyRepository _personReadOnlyRepository;
    private readonly ITransactionReadOnlyRepository _transactionReadOnlyRepository;

    public GetTransactionByIdUseCase(
        ICurrentUser currentUser,
        IPersonReadOnlyRepository personReadOnlyRepository,
        ITransactionReadOnlyRepository transactionReadOnlyRepository
        )
    {
        _currentUser = currentUser;
        _personReadOnlyRepository = personReadOnlyRepository;
        _transactionReadOnlyRepository = transactionReadOnlyRepository;
    }

    public async Task<TransactionResponse> Execute(Guid transactionId)
    {
        var currentUser = _currentUser.CurrentUser();
        var person = await _personReadOnlyRepository.GetByUserId(currentUser.Id) ?? throw new PersonNotFoundException();

        var transaction = await _transactionReadOnlyRepository.GetById(person: person, transactionId: transactionId)
            ?? throw new NotFoundException(ResourceMessagesException.TRANSACTION_NOT_FOUND);

        var response = transaction.Adapt<TransactionResponse>();

        return response;
    }
}
