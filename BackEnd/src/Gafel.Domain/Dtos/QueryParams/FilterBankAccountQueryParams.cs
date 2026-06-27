using Gafel.Domain.Enums;

namespace Gafel.Domain.Dtos.QueryParams;

public record FilterBankAccountQueryParams : OptionalPaginationQueryParams
{
    public BankAccountType? Type { get; init; }
    public string? BankAccountName { get; set; }
}
