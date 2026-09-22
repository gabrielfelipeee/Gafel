using Gafel.Domain.Enums;

namespace Gafel.Domain.Entities;

public class Category : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public CategoryType Type { get; set; }
    public string Icon { get; set; } = string.Empty;

    public Guid PersonId { get; set; } // FK
    public Guid? DefaultCategoryId { get; set; } // FK
}
