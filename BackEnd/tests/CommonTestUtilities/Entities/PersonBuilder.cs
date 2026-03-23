using Bogus;

namespace CommonTestUtilities.Entities;

public class PersonBuilder
{
    public static Gafel.Domain.Entities.Person Build()
    {
        return new Faker<Gafel.Domain.Entities.Person>()
            .RuleFor(person => person.Id, _ => 1)
            .RuleFor(person => person.FullName, faker => faker.Person.FullName)
            .RuleFor(person => person.UserId, _ => 1);
    }
}
