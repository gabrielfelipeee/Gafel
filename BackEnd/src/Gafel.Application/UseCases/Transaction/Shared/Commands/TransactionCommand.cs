namespace Gafel.Application.UseCases.Transaction.Shared.Commands;

public class TransactionCommand
{
    public string? Description { get; set; }
    public DateOnly Date { get; set; }
    public decimal Amount { get; set; }
    public bool IsSettled { get; set; }

    public Guid BankAccountId { get; set; }
    public Guid CategoryId { get; set; }
}
