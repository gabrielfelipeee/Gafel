namespace Gafel.Domain.Dtos.QueryParams;

public abstract record OptionalPaginationQueryParams
{
    public int? Offset { get; init; }
    public int? Limit { get; init; }
}
