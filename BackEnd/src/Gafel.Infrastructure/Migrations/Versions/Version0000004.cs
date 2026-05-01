using FluentMigrator;

namespace Gafel.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.CREATE_TABLE_CATEGORIES, "Criar tabela de categorias")]
public class Version0000004 : VersionBase
{
    public override void Up()
    {
        CreateTableWithDefaults(DatabaseTables.CATEGORIES)
            .WithColumn("name").AsString(100).NotNullable()
            .WithColumn("is_active").AsBoolean().NotNullable()
            .WithColumn("type").AsInt32().NotNullable()
            .WithColumn("icon").AsString(50).NotNullable()

            .WithColumn("person_id").AsInt64().NotNullable()
                .ForeignKey("fk_categories_people_id", DatabaseTables.PEOPLE, "id")

            .WithColumn("default_category_id").AsInt64().Nullable()
                .ForeignKey("fk_categories_default_categories_id", DatabaseTables.DEFAULT_CATEGORIES, "id");
    }
}
