namespace Gafel.Domain.Dtos.QueryParams;

public abstract record PaginationQueryParams
{
    public int Offset { get; init; } = 0;
    public int Limit { get; init; } = 20;
}
