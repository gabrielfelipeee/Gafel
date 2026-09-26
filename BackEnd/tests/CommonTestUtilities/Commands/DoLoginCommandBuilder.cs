using CommonTestUtilities.Helpers;
using Gafel.Application.UseCases.Auth.Login.DoLogin;

namespace CommonTestUtilities.Commands;

public class DoLoginCommandBuilder
{
    public static DoLoginCommand Build()
    {
        return FakerFactory.Create<DoLoginCommand>()
        .RuleFor(acc => acc.Email, faker => faker.Internet.Email())
        .RuleFor(acc => acc.Password, faker => PasswordGenerator.Generate(faker, passwordLength: 8, withNumber: true, withSpecialCharacter: true));
    }
}
