namespace Gafel.Domain.Entities;

public abstract class EntityBase
{
    public Guid Id { get; private set; } = Guid.CreateVersion7();
    public bool IsActive { get; set; } = true;
}
