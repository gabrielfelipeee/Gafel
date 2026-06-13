using Gafel.Domain.Enums;

namespace Gafel.Application.UseCases.BankAccount.Shared.Commands;

public class BankAccountCommand
{
    public string Name { get; set; } = string.Empty;
    public decimal InitialBalance { get; set; }
    public BankAccountType Type { get; set; }
}
