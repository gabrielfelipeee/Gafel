using Bogus;
using Bogus.Extensions.Brazil;
using Gafel.Application.UseCases.Account.UpdateProfile;

namespace CommonTestUtilities.Commands;

public static class UpdateProfileCommandBuilder
{
    public static UpdateProfileCommand Build(bool withCpf = false, bool withDateOfBirth = false)
    {
        var mock = new Faker<UpdateProfileCommand>("pt_BR")
        .RuleFor(person => person.FullName, faker => faker.Person.FullName)
        .RuleFor(person => person.Uf, faker => faker.Address.StateAbbr())
        .RuleFor(person => person.City, (faker, person) => faker.Address.City());

        if (withCpf)
            mock.RuleFor(person => person.Cpf, f => f.Person.Cpf());

        if (withDateOfBirth)
            mock.RuleFor(person => person.DateOfBirth, _ => DateOnly.FromDateTime(DateTime.Today.AddYears(-18)));

        return mock;
    }
}
