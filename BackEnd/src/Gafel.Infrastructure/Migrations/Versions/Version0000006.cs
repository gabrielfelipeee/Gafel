using FluentMigrator;

namespace Gafel.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.CREATE_TABLE_TRANSACTIONS, "Criar tabela de lançamentos")]
public class Version0000006 : VersionBase
{
    public override void Up()
    {
        CreateTableWithDefaults(DatabaseTables.TRANSACTIONS)
            .WithColumn("description").AsString(100).Nullable()
            .WithColumn("date").AsDate().NotNullable()
            .WithColumn("amount").AsDecimal(18, 2).NotNullable()
            .WithColumn("is_settled").AsBoolean().NotNullable()

            .WithColumn("person_id").AsGuid().NotNullable()
                .ForeignKey("fk_transactions_people_id", DatabaseTables.PEOPLE, "id")

            .WithColumn("bank_account_id").AsGuid().NotNullable()
                .ForeignKey("fk_transactions_bank_accounts_id", DatabaseTables.BANK_ACCOUNTS, "id")

            .WithColumn("category_id").AsGuid().NotNullable()
                .ForeignKey("fk_transactions_categories_id", DatabaseTables.CATEGORIES, "id");

        Create.Index("idx_transactions_bank_account_id")
            .OnTable(DatabaseTables.TRANSACTIONS)
            .OnColumn("bank_account_id");
    }
}
