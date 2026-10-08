using Gafel.Domain.Enums;

namespace Gafel.Application.UseCases.Transaction.Shared.Responses;

public class TransactionBankAccountResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public BankAccountType Type { get; set; }
}
