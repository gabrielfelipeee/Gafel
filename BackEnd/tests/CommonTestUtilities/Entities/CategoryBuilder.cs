using Bogus;
using CommonTestUtilities.Helpers;
using Gafel.Domain.Entities;
using Gafel.Domain.Enums;

namespace CommonTestUtilities.Entities;

public class CategoryBuilder
{

    public static IList<Category> Collection(Gafel.Domain.Entities.Person person, uint count = 2)
    {
        var list = new List<Category>();

        if (count == 0)
            count = 1;

        var categoryId = 1;

        for (int i = 0; i < count; i++)
        {
            var fakeCategory = Build(person);
            fakeCategory.Id = categoryId++;

            list.Add(fakeCategory);
        }

        return list;
    }

    public static Category Build(Gafel.Domain.Entities.Person person)
    {

        return new Faker<Category>()
            .CustomInstantiator(faker =>
            {
                var type = faker.PickRandom<CategoryType>();

                var source = type == CategoryType.Expense ? CategoryData.Expense : CategoryData.Income;

                var (Name, Icon) = faker.PickRandom(source);

                return new()
                {
                    Type = type,
                    Name = Name,
                    Icon = Icon,
                    PersonId = person.Id,
                };
            });
    }
}
