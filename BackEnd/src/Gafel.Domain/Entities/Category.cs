using Gafel.Domain.Enums;

namespace Gafel.Domain.Entities;

public class Category : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public CategoryType Type { get; set; }
    public string Icon { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public long PersonId { get; set; } // FK
    public long? DefaultCategoryId { get; set; } // FK
}
