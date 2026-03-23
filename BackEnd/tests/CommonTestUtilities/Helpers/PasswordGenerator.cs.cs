using Bogus;

namespace CommonTestUtilities.Helpers;

public static class PasswordGenerator
{
    public static string Generate(
    Faker faker,
    uint passwordLength = 8,
    bool withNumber = true,
    bool withSpecialCharacter = true)
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
