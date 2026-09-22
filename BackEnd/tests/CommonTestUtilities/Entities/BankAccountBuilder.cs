using Bogus;
using Gafel.Domain.Entities;
using Gafel.Domain.Enums;

namespace CommonTestUtilities.Entities;

public class BankAccountBuilder
{

    public static IList<BankAccount> Collection(Gafel.Domain.Entities.Person person, uint count = 2)
    {
        var list = new List<BankAccount>();

        if (count == 0)
            count = 1;

        for (int i = 0; i < count; i++)
        {
            var fakeBankAccount = Build(person);

            list.Add(fakeBankAccount);
        }

        return list;
    }

    public static BankAccount Build(Gafel.Domain.Entities.Person person)
    {
        return new Faker<BankAccount>("pt_BR")
            .RuleFor(bankAccount => bankAccount.Id, _ => Guid.CreateVersion7())
            .RuleFor(bankAccount => bankAccount.PersonId, _ => person.Id)
            .RuleFor(bankAccount => bankAccount.Name, faker => faker.Company.CompanyName())
            .RuleFor(bankAccount => bankAccount.InitialBalance, faker => faker.Finance.Amount(0, 10000))
            .RuleFor(bankAccount => bankAccount.Type, faker => faker.PickRandom<BankAccountType>());
    }
}
