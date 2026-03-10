using FluentMigrator;
using FluentMigrator.Builders.Create.Table;

namespace Gafel.Infrastructure.Migrations.Versions;

public abstract class VersionBase : ForwardOnlyMigration
{
    protected ICreateTableColumnOptionOrWithColumnSyntax CreateTableWithDefaults(string tableName)
    {
        return Create.Table(tableName)
            .WithColumn("id").AsInt64().PrimaryKey().Identity()
            .WithColumn("created_at").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
            .WithColumn("updated_at").AsDateTime().Nullable();
    }
}
