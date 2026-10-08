using Gafel.Application.UseCases.Shared.Responses;

namespace Gafel.Application.UseCases.Transaction.Shared.Responses;

public class TransactionResponse
{
    public Guid Id { get; set; }
    public string? Description { get; set; }
    public DateOnly Date { get; set; }
    public decimal Amount { get; set; }
    public bool IsSettled { get; set; } = true;

    public virtual TransactionBankAccountResponse BankAccount { get; set; } = default!;
    public virtual CategoryResponse Category { get; set; } = default!;
}