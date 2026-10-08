using Gafel.Domain.Enums;

namespace Gafel.Application.UseCases.Shared.Responses;

public class CategoryResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public CategoryType Type { get; set; }
}
