using Bogus;
using Bogus.Extensions.Brazil;
using Gafel.Domain.Enums;
using Gafel.Domain.ValueObjects;

namespace CommonTestUtilities.Entities;

public class PersonBuilder
{
    public static Gafel.Domain.Entities.Person Build(bool withCpf = false, bool withDateOfBirth = false)
    {
        return new Faker<Gafel.Domain.Entities.Person>("pt_BR")
            .RuleFor(person => person.Id, _ => 1)
            .RuleFor(person => person.FullName, faker => faker.Person.FullName)
            .RuleFor(person => person.Uf, f => Enum.Parse<Uf>(f.Address.StateAbbr()))
            .RuleFor(person => person.City, faker => faker.Address.City())
            .RuleFor(person => person.UserId, _ => 1)
            .FinishWith((faker, person) =>
            {
                if (withCpf)
                {
                    Cpf cpf = Cpf.Create(faker.Person.Cpf()).Value!;
                    person.SetCpf(cpf);
                }

                if (withDateOfBirth)
                {
                    var date = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-18));

                    DateOfBirth dateOfBirth = DateOfBirth.Create(date).Value!;
                    person.SetDateOfBirth(dateOfBirth);
                }

            });
    }
}
