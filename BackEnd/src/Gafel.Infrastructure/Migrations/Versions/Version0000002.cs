using FluentMigrator;

namespace Gafel.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.CREATE_TABLE_PEOPLE, "Criar tabela de pessoas")]
public class Version0000002 : VersionBase
{
    public override void Up()
    {
        CreateTableWithDefaults(DatabaseTables.PEOPLE)
            .WithColumn("full_name").AsString(60).NotNullable()
            .WithColumn("cpf").AsFixedLengthString(11).Nullable()
            .WithColumn("date_of_birth").AsDate().Nullable()
            .WithColumn("city").AsString(60).Nullable()
            .WithColumn("uf").AsFixedLengthString(2).Nullable()
            .WithColumn("user_id").AsInt64().NotNullable()
                .ForeignKey("fk_people_users_id", DatabaseTables.USERS, "id");

        Create.Index("idx_people_cpf")
            .OnTable(DatabaseTables.PEOPLE)
            .OnColumn("cpf")
            .Unique();
    }
}
