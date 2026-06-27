using Gafel.Application.Exceptions;
using Gafel.Application.Extensions;
using Gafel.Application.UseCases.BankAccount.Shared.Responses;
using Gafel.Application.UseCases.Shared.Responses;
using Gafel.Domain.Dtos.QueryParams;
using Gafel.Domain.Repositories.BankAccount;
using Gafel.Domain.Repositories.Person;
using Gafel.Domain.Services.CurrentUser;
using Mapster;

namespace Gafel.Application.UseCases.BankAccount.Filter;

public class FilterBankAccountUseCase : IFilterBankAccountUseCase
{
    private readonly ICurrentUser _currentUser;
    private readonly IPersonReadOnlyRepository _personReadOnlyRepository;
    private readonly IBankAccountReadOnlyRepository _bankAccountReadOnlyRepository;

    public FilterBankAccountUseCase(
        ICurrentUser currentUser,
        IPersonReadOnlyRepository personReadOnlyRepository,
        IBankAccountReadOnlyRepository bankAccountReadOnlyRepository
        )
    {
        _currentUser = currentUser;
        _personReadOnlyRepository = personReadOnlyRepository;
        _bankAccountReadOnlyRepository = bankAccountReadOnlyRepository;
    }

    public async Task<PaginationResponse<BankAccountResponse>> Execute(FilterBankAccountQueryParams query)
    {
        Validate(query);

        var currentUser = _currentUser.CurrentUser();
        var person = await _personReadOnlyRepository.GetByUserId(currentUser.Id) ?? throw new PersonNotFoundException();

        bool isPaged = query.Offset.HasValue || query.Limit.HasValue;

        var (bankAccounts, total) = await _bankAccountReadOnlyRepository.Filter(person: person, filters: query);

        return new()
        {
            Items = bankAccounts.Adapt<IList<BankAccountResponse>>(),
            Offset = isPaged ? query.Offset!.Value : 0,
            Limit = isPaged ? query.Limit!.Value : total,
            Total = total
        };
    }


    private static void Validate(FilterBankAccountQueryParams query)
    {
        var result = new FilterBankAccountQueryParamsValidator().Validate(query);

        if (!result.IsValid)
        {
            var errors = result.Errors.ToErrorsByPropertyDictionary();

            throw new ErrorOnValidationException(errors);
        }
    }
}
