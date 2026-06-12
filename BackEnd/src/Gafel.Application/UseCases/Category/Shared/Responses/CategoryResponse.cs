using Gafel.Domain.Enums;

namespace Gafel.Application.UseCases.Category.Shared.Responses;

public class CategoryResponse
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public CategoryType Type { get; set; }
}
