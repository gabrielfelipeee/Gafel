using FluentMigrator;
using FluentMigrator.Builders.Create.Table;

namespace Gafel.Infrastructure.Migrations.Versions;

public abstract class VersionBase : ForwardOnlyMigration
{
    protected ICreateTableColumnOptionOrWithColumnSyntax CreateTableWithDefaults(string tableName)
    {
        return Create.Table(tableName)
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("is_active").AsBoolean().NotNullable();
    }
}
