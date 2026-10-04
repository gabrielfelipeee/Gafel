using Gafel.Application.UseCases.Transaction.Shared.Commands;
using Gafel.Domain.Constants;

namespace CommonTestUtilities.Commands;

public class TransactionCommandBuilder
{
    public static TransactionCommand Build(Guid? bankAccountId = null, Guid? categoryId = null)
    {
        return FakerFactory.Create<TransactionCommand>()
        .RuleFor(transaction => transaction.Date, () => DateOnly.FromDateTime(DateTime.UtcNow))
        .RuleFor(transaction => transaction.Amount, faker => faker.Finance.Amount(min: 0.01m, max: DomainRules.MaximumMoneyAmount))
        .RuleFor(transaction => transaction.IsSettled, faker => faker.Random.Bool())
        .RuleFor(transaction => transaction.BankAccountId, () => bankAccountId ?? Guid.CreateVersion7())
        .RuleFor(transaction => transaction.CategoryId, () => categoryId ?? Guid.CreateVersion7());
    }
}
