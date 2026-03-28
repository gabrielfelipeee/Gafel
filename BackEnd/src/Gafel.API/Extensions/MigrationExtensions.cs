using Gafel.Infrastructure.Extensions;
using Gafel.Infrastructure.Migrations;
using Gafel.Infrastructure.Services.Identity.Seed;

namespace Gafel.API.Extensions;

public static class MigrationExtensions
{
    public static async Task UseDatabaseInitialization(this WebApplication app)
    {
        if (app.Configuration.IsUnitTestEnvironment())
            return;

        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;

        // Executa as Migrations
        var connectionString = app.Configuration.ConnectionString();
        DatabaseMigration.Migrate(services, connectionString);

        // Executa o Seed
        await IdentitySeeder.SeedRolesAsync(services);
    }
}
