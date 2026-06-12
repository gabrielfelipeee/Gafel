using Gafel.Domain.Entities;
using Gafel.Infrastructure.Migrations;
using Gafel.Infrastructure.Services.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Gafel.Infrastructure.DataAccess;

public class GafelDbContext(DbContextOptions<GafelDbContext> options) : IdentityDbContext<ApplicationUser, IdentityRole<long>, long>(options)
{

    public DbSet<Person> People { get; set; }
    public DbSet<DefaultCategory> DefaultCategories { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>().ToTable(DatabaseTables.USERS);
        builder.Entity<IdentityRole<long>>().ToTable(DatabaseTables.ROLES);
        builder.Entity<IdentityUserRole<long>>().ToTable(DatabaseTables.USER_ROLES);
        builder.Entity<IdentityUserClaim<long>>().ToTable(DatabaseTables.USER_CLAIMS);
        builder.Entity<IdentityUserLogin<long>>().ToTable(DatabaseTables.USER_LOGINS);
        builder.Entity<IdentityUserToken<long>>().ToTable(DatabaseTables.USER_TOKENS);
        builder.Entity<IdentityRoleClaim<long>>().ToTable(DatabaseTables.ROLE_CLAIMS);

        builder.Entity<Person>().ToTable(DatabaseTables.PEOPLE);
        builder.Entity<DefaultCategory>().ToTable(DatabaseTables.DEFAULT_CATEGORIES);
        builder.Entity<Category>().ToTable(DatabaseTables.CATEGORIES);
        builder.Entity<BankAccount>().ToTable(DatabaseTables.BANK_ACCOUNTS);

        // Aplica todas as configurações de mapeamento de entidades para o modelo de dados, que estão no mesmo assembly.
        builder.ApplyConfigurationsFromAssembly(typeof(GafelDbContext).Assembly);
    }
}
