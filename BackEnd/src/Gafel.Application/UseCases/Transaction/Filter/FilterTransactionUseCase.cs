using Gafel.Application.Exceptions;
using Gafel.Application.Extensions;
using Gafel.Application.UseCases.Shared.Responses;
using Gafel.Application.UseCases.Transaction.Shared.Responses;
using Gafel.Domain.Dtos.QueryParams;
using Gafel.Domain.Repositories.Person;
using Gafel.Domain.Repositories.Transaction;
using Gafel.Domain.Services.CurrentUser;
using Mapster;

namespace Gafel.Application.UseCases.Transaction.Filter;

public class FilterTransactionUseCase : IFilterTransactionUseCase
{
    private static readonly TimeZoneInfo BrazilTimeZone = TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");

    private readonly ICurrentUser _currentUser;
    private readonly IPersonReadOnlyRepository _personReadOnlyRepository;
    private readonly ITransactionReadOnlyRepository _transactionReadOnlyRepository;
    private readonly TimeProvider _timeProvider;

    public FilterTransactionUseCase(
        TimeProvider timeProvider,
        ICurrentUser currentUser,
        IPersonReadOnlyRepository personReadOnlyRepository,
        ITransactionReadOnlyRepository transactionReadOnlyRepository
        )
    {
        _timeProvider = timeProvider;
        _currentUser = currentUser;
        _personReadOnlyRepository = personReadOnlyRepository;
        _transactionReadOnlyRepository = transactionReadOnlyRepository;
    }

    public async Task<PaginationResponse<TransactionResponse>> Execute(FilterTransactionQueryParams query)
    {
        Validate(query);

        var currentUser = _currentUser.CurrentUser();
        var person = await _personReadOnlyRepository.GetByUserId(currentUser.Id) ?? throw new PersonNotFoundException();

        var (startDate, endDate) = ResolvePeriod(query);
        query.StartDate = startDate;
        query.EndDate = endDate;

        var (transactions, total) = await _transactionReadOnlyRepository.Filter(person: person, filters: query);

        return new()
        {
            Items = transactions.Adapt<IList<TransactionResponse>>(),
            Offset = query.Offset,
            Limit = query.Limit,
            Total = total
        };
    }

    private (DateOnly startDate, DateOnly endDate) ResolvePeriod(FilterTransactionQueryParams query)
    {
        if (query.StartDate.HasValue && query.EndDate.HasValue)
            return (query.StartDate.Value, query.EndDate.Value);

        var localNow = TimeZoneInfo.ConvertTime(_timeProvider.GetUtcNow(), BrazilTimeZone);
        var today = DateOnly.FromDateTime(localNow.DateTime);

        var startDate = query.StartDate ?? new DateOnly(today.Year, today.Month, 1);
        var endDate = query.EndDate ?? new DateOnly(startDate.Year, startDate.Month, DateTime.DaysInMonth(startDate.Year, startDate.Month));

        return (startDate, endDate);
    }


    private static void Validate(FilterTransactionQueryParams query)
    {
        var result = new FilterTransactionQueryParamsValidator().Validate(query);

        if (!result.IsValid)
        {
            var errors = result.Errors.ToErrorsByPropertyDictionary();

            throw new ErrorOnValidationException(errors);
        }
    }
}
