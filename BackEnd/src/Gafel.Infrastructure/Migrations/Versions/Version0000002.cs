using FluentMigrator;

namespace Gafel.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.CREATE_TABLE_PEOPLE, "Criar tabela de pessoas")]
public class Version0000002 : VersionBase
{
    public override void Up()
    {
        CreateTableWithDefaults("people")
            .WithColumn("full_name").AsString(60).NotNullable()
            .WithColumn("cpf").AsString(11).Nullable()
            .WithColumn("date_of_birth").AsDate().Nullable()
            .WithColumn("city").AsString(60).Nullable()
            .WithColumn("uf").AsString(2).Nullable()
            .WithColumn("user_id").AsInt64().NotNullable()
                .ForeignKey("fk_people_users_id", "users", "id");

        Create.Index("idx_people_cpf")
            .OnTable("people")
            .OnColumn("cpf")
            .Unique();
    }
}
