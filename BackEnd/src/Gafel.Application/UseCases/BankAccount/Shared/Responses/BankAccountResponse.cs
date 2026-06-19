using Gafel.Domain.Enums;

namespace Gafel.Application.UseCases.BankAccount.Shared.Responses;

public class BankAccountResponse
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal InitialBalance { get; set; }
    public BankAccountType Type { get; set; }
}
