using Bogus;
using Gafel.Application.UseCases.Category.Shared.Commands;
using Gafel.Domain.Enums;

namespace CommonTestUtilities.Commands;

public class CategoryCommandBuilder
{
    private static readonly (string Name, string Icon)[] Expense =
        [
            ("Alimentação", "restaurant"),
            ("Transporte", "directions_car"),
            ("Moradia", "home"),
            ("Saúde", "local_hospital"),
            ("Educação", "school"),
            ("Lazer", "sports_esports"),
            ("Assinaturas", "subscriptions"),
            ("Contas", "receipt"),
            ("Compras", "shopping_cart")
        ];

    private static readonly (string Name, string Icon)[] Income =
    [
        ("Salário", "attach_money"),
        ("Freelance", "work"),
        ("Investimentos", "trending_up"),
        ("Venda", "sell"),
        ("Bônus", "card_giftcard"),
        ("Reembolso", "payments")
    ];

    public static CategoryCommand Build()
    {
        return new Faker<CategoryCommand>()
            .CustomInstantiator(faker =>
            {
                var type = faker.PickRandom<CategoryType>();

                var source = type == CategoryType.Expense ? Expense : Income;

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
