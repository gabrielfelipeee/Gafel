using Bogus;
using Bogus.Extensions.Brazil;
using Gafel.Application.UseCases.Person.Update;

namespace CommonTestUtilities.Commands;

public static class UpdatePersonCommandBuilder
{
    public static UpdatePersonCommand Build()
    {
        return new Faker<UpdatePersonCommand>("pt_BR")
        .RuleFor(person => person.FullName, faker => faker.Person.FullName)
        .RuleFor(person => person.Cpf, faker => faker.Person.Cpf())
        .RuleFor(person => person.DateOfBirth, _ => DateOnly.FromDateTime(DateTime.Today.AddYears(-18)))
        .RuleFor(person => person.Uf, faker => faker.Address.StateAbbr())
        .RuleFor(person => person.City, (faker, person) => faker.Address.City());
    }
}
