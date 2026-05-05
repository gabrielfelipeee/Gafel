namespace CommonTestUtilities.Helpers;

public static class CategoryData
{
    public static readonly (string Name, string Icon)[] Expense =
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

    public static readonly (string Name, string Icon)[] Income =
        [
            ("Salário", "attach_money"),
            ("Freelance", "work"),
            ("Investimentos", "trending_up"),
            ("Venda", "sell"),
            ("Bônus", "card_giftcard"),
            ("Reembolso", "payments")
        ];
}
