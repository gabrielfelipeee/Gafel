using Gafel.Domain.Enums;

namespace Gafel.Domain.Dtos.QueryParams;

public record FilterTransactionQueryParams : PaginationQueryParams
{
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }

    public CategoryType? Type { get; set; }
    public bool? IsSettled { get; set; }
    public decimal? MinimumAmount { get; set; }
    public decimal? MaximumAmount { get; set; }

    public Guid? BankAccountId { get; set; }
    public Guid? CategoryId { get; set; }
}
