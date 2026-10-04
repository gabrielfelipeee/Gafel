namespace Gafel.Domain.Entities;

public class Transaction : EntityBase
{
    public string? Description { get; set; }
    public DateOnly Date { get; set; }
    public decimal Amount { get; set; }
    public bool IsSettled { get; set; } = true;

    // FKs
    public Guid PersonId { get; set; }
    public Guid BankAccountId { get; set; }
    public Guid CategoryId { get; set; }

    public virtual BankAccount BankAccount { get; set; } = default!;
    public virtual Category Category { get; set; } = default!;
}
