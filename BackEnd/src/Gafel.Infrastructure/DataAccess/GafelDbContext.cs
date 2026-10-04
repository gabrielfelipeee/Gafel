using Gafel.Domain.Entities;
using Gafel.Infrastructure.Migrations;
using Gafel.Infrastructure.Services.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Gafel.Infrastructure.DataAccess;

public class GafelDbContext(DbContextOptions<GafelDbContext> options) : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options)
{

    public DbSet<Person> People { get; set; }
    public DbSet<DefaultCategory> DefaultCategories { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<BankAccount> BankAccounts { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>().ToTable(DatabaseTables.USERS);
        builder.Entity<IdentityRole<Guid>>().ToTable(DatabaseTables.ROLES);
        builder.Entity<IdentityUserRole<Guid>>().ToTable(DatabaseTables.USER_ROLES);
        builder.Entity<IdentityUserClaim<Guid>>().ToTable(DatabaseTables.USER_CLAIMS);
        builder.Entity<IdentityUserLogin<Guid>>().ToTable(DatabaseTables.USER_LOGINS);
        builder.Entity<IdentityUserToken<Guid>>().ToTable(DatabaseTables.USER_TOKENS);
        builder.Entity<IdentityRoleClaim<Guid>>().ToTable(DatabaseTables.ROLE_CLAIMS);

        builder.Entity<Person>().ToTable(DatabaseTables.PEOPLE);
        builder.Entity<DefaultCategory>().ToTable(DatabaseTables.DEFAULT_CATEGORIES);
        builder.Entity<Category>().ToTable(DatabaseTables.CATEGORIES);
        builder.Entity<BankAccount>().ToTable(DatabaseTables.BANK_ACCOUNTS);
        builder.Entity<Transaction>().ToTable(DatabaseTables.TRANSACTIONS);

        // Aplica todas as configurações de mapeamento de entidades para o modelo de dados, que estão no mesmo assembly.
        builder.ApplyConfigurationsFromAssembly(typeof(GafelDbContext).Assembly);
    }
}
