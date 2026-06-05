using Gafel.Domain.Enums;

namespace Gafel.Domain.Dtos.QueryParams;

public record FilterCategoryQueryParams : OptionalPaginationQueryParams
{
    public CategoryType? Type { get; init; }
    public string? CategoryName { get; set; }
}