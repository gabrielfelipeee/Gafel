namespace Gafel.Domain.Dtos.QueryParams;

public abstract record PaginationQueryParams
{
    public int Offset { get; init; }
    public int Limit { get; init; }
}
