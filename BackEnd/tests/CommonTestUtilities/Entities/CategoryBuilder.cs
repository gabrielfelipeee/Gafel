using CommonTestUtilities.Commands;
using Gafel.Domain.Entities;

namespace CommonTestUtilities.Entities;

public class CategoryBuilder
{
    public static Category Build(Person person)
    {
        var categoryComamnd = CategoryCommandBuilder.Build();

        return new Category()
        {
            Id = 1,
            Name = categoryComamnd.Name,
            Icon = categoryComamnd.Icon,
            PersonId = person.Id,
            Type = categoryComamnd.Type
        };
    }
}
