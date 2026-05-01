using Gafel.Domain.Enums;

namespace Gafel.Application.UseCases.Category.Shared.Commands;

public class CategoryCommand
{
    public string Name { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public CategoryType Type { get; set; }
}
