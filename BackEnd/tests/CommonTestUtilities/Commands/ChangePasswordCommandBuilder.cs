using Bogus;
using CommonTestUtilities.Helpers;
using Gafel.Application.UseCases.Auth.ChangePassword;

namespace CommonTestUtilities.Commands;

public class ChangePasswordCommandBuilder
{
    public static ChangePasswordCommand Build()
    {
        return new Faker<ChangePasswordCommand>()
        .RuleFor(acc => acc.Password, faker => PasswordGenerator.Generate(faker, passwordLength: 8, withNumber: true, withSpecialCharacter: true))
        .RuleFor(acc => acc.NewPassword, faker => PasswordGenerator.Generate(faker, passwordLength: 8, withNumber: true, withSpecialCharacter: true));
    }
}
