using Gafel.Domain.Enums;

namespace Gafel.Domain.Entities;

public class BankAccount : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public decimal InitialBalance { get; set; }
    public BankAccountType Type { get; set; }
    public bool IsActive { get; set; } = true;

    public long PersonId { get; set; } // FK
}
