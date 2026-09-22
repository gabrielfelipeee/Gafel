using FluentMigrator;

namespace Gafel.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.CREATE_TABLE_BANK_ACCOUNTS, "Criar tabela de contas bancarias")]
public class Version0000005 : VersionBase
{
    public override void Up()
    {
        CreateTableWithDefaults(DatabaseTables.BANK_ACCOUNTS)
            .WithColumn("name").AsString(100).NotNullable()
            .WithColumn("initial_balance").AsDecimal(18, 2).NotNullable()
            .WithColumn("type").AsInt32().NotNullable()

            .WithColumn("person_id").AsGuid().NotNullable()
                .ForeignKey("fk_bank_accounts_people_id", DatabaseTables.PEOPLE, "id");
    }
}
