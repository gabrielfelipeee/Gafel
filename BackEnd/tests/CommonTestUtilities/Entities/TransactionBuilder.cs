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

    public static Transaction Build(Person person, Guid? bankAccountId = null, Guid? categoryId = null, bool include = false)
    {
        return FakerFactory.Create<Transaction>()
        .RuleFor(transaction => transaction.Date, () => DateOnly.FromDateTime(DateTime.UtcNow))
        .RuleFor(transaction => transaction.Amount, faker => faker.Finance.Amount(min: DomainRules.MinimumTransactionAmount, max: DomainRules.MaximumMoneyAmount))
        .RuleFor(transaction => transaction.IsSettled, faker => faker.Random.Bool())
        .RuleFor(transaction => transaction.PersonId, () => person.Id)
        .FinishWith((faker, transaction) =>
        {
            if (include)
            {
                var bankAccount = BankAccountBuilder.Build(person);
                var category = CategoryBuilder.Build(person);

                transaction.BankAccount = bankAccount;
                transaction.BankAccountId = bankAccount.Id;

                transaction.Category = category;
                transaction.CategoryId = category.Id;
                return;
            }

            transaction.BankAccountId = bankAccountId ?? Guid.CreateVersion7();
            transaction.CategoryId = categoryId ?? Guid.CreateVersion7();
        });
    }
}
