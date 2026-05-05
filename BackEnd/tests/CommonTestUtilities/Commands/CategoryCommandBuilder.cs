using Bogus;
using CommonTestUtilities.Helpers;
using Gafel.Application.UseCases.Category.Shared.Commands;
using Gafel.Domain.Enums;

namespace CommonTestUtilities.Commands;

public class CategoryCommandBuilder
{
    public static CategoryCommand Build()
    {
        return new Faker<CategoryCommand>()
            .CustomInstantiator(faker =>
            {
                var type = faker.PickRandom<CategoryType>();

                var source = type == CategoryType.Expense ? CategoryData.Expense : CategoryData.Income;

                var (Name, Icon) = faker.PickRandom(source);

                return new CategoryCommand
                {
                    Type = type,
                    Name = Name,
                    Icon = Icon
                };
            });
    }
}
