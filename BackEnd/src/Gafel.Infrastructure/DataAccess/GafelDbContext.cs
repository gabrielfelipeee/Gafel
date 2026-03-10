using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Gafel.Domain.Entities;
using Gafel.Infrastructure.Identity.Entities;

namespace Gafel.Infrastructure.DataAccess;

public class GafelDbContext(DbContextOptions<GafelDbContext> options) : IdentityDbContext<ApplicationUser, IdentityRole<long>, long>(options)
{

    public DbSet<Person> Peoples { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ApplicationUser>().ToTable("users");
        modelBuilder.Entity<IdentityRole<long>>().ToTable("roles");
        modelBuilder.Entity<IdentityUserRole<long>>().ToTable("user_roles");
        modelBuilder.Entity<IdentityUserClaim<long>>().ToTable("user_claims");
        modelBuilder.Entity<IdentityUserLogin<long>>().ToTable("user_logins");
        modelBuilder.Entity<IdentityUserToken<long>>().ToTable("user_tokens");
        modelBuilder.Entity<IdentityRoleClaim<long>>().ToTable("role_claims");

        // Aplica todas as configurações de mapeamento de entidades para o modelo de dados, que estão no mesmo assembly.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GafelDbContext).Assembly);
    }
}

