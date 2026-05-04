namespace Gafel.Application.UseCases.Shared.Responses;

public class PaginationResponse<T>
{
    public IList<T> Items { get; set; } = [];
    public int Total { get; set; }
    public int Offset { get; set; }
    public int Limit { get; set; }
}
