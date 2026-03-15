using Bogus;
using Gafel.Application.UseCases.Auth.Register;

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
        .RuleFor(acc => acc.Password, faker => GeneratePassword(faker, passwordLength, withNumber, withSpecialCharacter));
    }

    private static string GeneratePassword(
        Faker faker,
        uint passwordLength,
        bool withNumber,
        bool withSpecialCharacter)
    {
        var password = new List<char>();

        if (withNumber && passwordLength >= 1)
            password.Add(faker.Random.Char('0', '9'));

        if (withSpecialCharacter && passwordLength >= 2)
            password.Add(faker.Random.Char('!', '*'));

        // Preenche o restante com letras minúsculas aleatórias
        while (password.Count < passwordLength)
            password.Add(faker.Random.Char('a', 'z'));

        // Embaralha para que o número/especial não fiquem sempre no início
        return new string([.. faker.Random.Shuffle(password)]);
    }
}
