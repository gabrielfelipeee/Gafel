using Bogus;
using Gafel.Application.UseCases.BankAccount.Shared.Commands;
using Gafel.Domain.Enums;

namespace CommonTestUtilities.Commands;

public class BankAccountCommandBuilder
{
    public static BankAccountCommand Build()
    {
        return new Faker<BankAccountCommand>("pt_BR")
        .RuleFor(bankAccount => bankAccount.Name, faker => faker.Company.CompanyName())
        .RuleFor(bankAccount => bankAccount.InitialBalance, faker => faker.Finance.Amount(0, 10000))
        .RuleFor(bankAccount => bankAccount.Type, faker => faker.PickRandom<BankAccountType>());
    }
}
