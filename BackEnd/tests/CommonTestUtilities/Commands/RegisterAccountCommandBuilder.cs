using Bogus;
using CommonTestUtilities.Helpers;
using Gafel.Application.UseCases.Category.Shared.Commands;

namespace CommonTestUtilities.Commands;

public static class RegisterAccountCommandBuilder
{
    public static RegisterAccountCommand Build(
        uint passwordLength = 8,
        bool withNumber = true,
        bool withSpecialCharacter = true)
    {
        return new Faker<RegisterAccountCommand>("pt_BR")
        .RuleFor(acc => acc.FullName, faker => faker.Person.FullName)
        .RuleFor(acc => acc.Email, (faker, acc) => faker.Internet.Email(acc.FullName))
        .RuleFor(acc => acc.Password, faker => PasswordGenerator.Generate(faker, passwordLength, withNumber, withSpecialCharacter));
    }
}
