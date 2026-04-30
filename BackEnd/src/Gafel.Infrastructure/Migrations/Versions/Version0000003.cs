using FluentMigrator;

namespace Gafel.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.CREATE_TABLE_DEFAULT_CATEGORIES, "Criar tabela de categorias padrão")]
public class Version0000003 : VersionBase
{
    public override void Up()
    {
        CreateTableWithDefaults(DatabaseTables.DEFAULT_CATEGORIES)
            .WithColumn("name").AsString(100).NotNullable()
            .WithColumn("is_active").AsBoolean().NotNullable()
            .WithColumn("type").AsInt64().NotNullable()
            .WithColumn("icon").AsString(50).NotNullable();
    }
}
