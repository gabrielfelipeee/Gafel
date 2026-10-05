using Gafel.Domain.Constants;
using Gafel.Domain.Entities;

namespace CommonTestUtilities.Entities;

public class TransactionBuilder
{

    public static IList<Transaction> Collection(Person person, uint count = 2)
    {
        var list = new List<Transaction>();

        if (count == 0)
            count = 1;

        for (int i = 0; i < count; i++)
        {
            var fakeBankAccount = Build(person);

            list.Add(fakeBankAccount);
        }

        return list;
    }

    public static Transaction Build(Person person, Guid? bankAccountId = null, Guid? categoryId = null)
    {
        return FakerFactory.Create<Transaction>()
        .RuleFor(transaction => transaction.Date, () => DateOnly.FromDateTime(DateTime.UtcNow))
        .RuleFor(transaction => transaction.Amount, faker => faker.Finance.Amount(min: 0.01m, max: DomainRules.MaximumMoneyAmount))
        .RuleFor(transaction => transaction.IsSettled, faker => faker.Random.Bool())
        .RuleFor(transaction => transaction.PersonId, () => person.Id)
        .RuleFor(transaction => transaction.BankAccountId, () => bankAccountId ?? Guid.CreateVersion7())
        .RuleFor(transaction => transaction.CategoryId, () => categoryId ?? Guid.CreateVersion7());
    }
}
