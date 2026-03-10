using FluentMigrator.Runner;
using Gafel.Domain.Identity.Interfaces;
using Gafel.Domain.Repositories;
using Gafel.Domain.Repositories.Person;
using Gafel.Infrastructure.DataAccess;
using Gafel.Infrastructure.DataAccess.Repositories;
using Gafel.Infrastructure.Extensions;
using Gafel.Infrastructure.Identity;
using Gafel.Infrastructure.Identity.Entities;
using Gafel.Infrastructure.Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Gafel.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddDbContextMySql(services, configuration);
        AddFluentMigratorMySql(services, configuration);

        AddIdentity(services);
        AddIdentityService(services);
        AddRepositories(services);
    }

    private static void AddIdentity(IServiceCollection services)
    {
        // Identity 
        services
            .AddIdentity<ApplicationUser, IdentityRole<long>>(options =>
            {
                // Comprexidade da senha
                options.Password.RequiredLength = 8; // Comprimento mínimo da senha
                options.Password.RequireDigit = true; // Pelo menos um número
                options.Password.RequireNonAlphanumeric = true; // Pelo menos um caractere não alfanumérico (especial)
                options.Password.RequireLowercase = false; // Pelo menos uma letra minúscula
                options.Password.RequireUppercase = false; // Pelo menos uma letra maiúscula
            })
            .AddErrorDescriber<CustomIdentityErrorDescriber>()
            .AddEntityFrameworkStores<GafelDbContext>()
            .AddDefaultTokenProviders();
    }

    private static void AddIdentityService(IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserWriteOnlyService, IdentityUserService>();
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IPersonReadOnlyRepository, PersonRepository>();
        services.AddScoped<IPersonWriteOnlyRepository, PersonRepository>();
    }


    private static void AddDbContextMySql(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.ConnectionString();
        var serverVersion = ServerVersion.AutoDetect(connectionString);

        services.AddDbContext<GafelDbContext>(options =>
        {
            options.UseMySql(connectionString, serverVersion)
            .UseSnakeCaseNamingConvention();
        });
    }

    // Método responsável por configurar o FluentMigrator para utilizar MySQL
    private static void AddFluentMigratorMySql(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.ConnectionString();

        // Registra os serviços principais do FluentMigrator no container de injeção de dependência
        services.AddFluentMigratorCore().ConfigureRunner(options =>
        {
            options
                .AddMySql8()     // Define o banco de dados como MySQL 5.x
                .WithGlobalConnectionString(connectionString)// Define a string de conexão que será usada pelas migrations

                // Define o assembly onde estão localizadas as classes de migration
                // Aqui ele carrega dinamicamente o assembly chamado "Gafel.Infrastructure"
                // e escaneia todas as classes que implementam migrations
                .ScanIn(Assembly.Load("Gafel.Infrastructure")).For.All();
        });
    }
}
