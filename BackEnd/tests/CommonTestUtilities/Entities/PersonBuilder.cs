using Bogus.Extensions.Brazil;
using Gafel.Domain.Enums;
using Gafel.Domain.ValueObjects;

namespace CommonTestUtilities.Entities;

public class PersonBuilder
{
    public static Gafel.Domain.Entities.Person Build(Gafel.Domain.Dtos.UserDto user, bool withCpf = false, bool withDateOfBirth = false)
    {
        return FakerFactory.Create<Gafel.Domain.Entities.Person>()
            .RuleFor(person => person.FullName, faker => faker.Person.FullName)
            .RuleFor(person => person.Uf, faker => faker.Random.Enum<Uf>())
            .RuleFor(person => person.City, faker => faker.Address.City())
            .RuleFor(person => person.UserId, _ => user.Id)
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
