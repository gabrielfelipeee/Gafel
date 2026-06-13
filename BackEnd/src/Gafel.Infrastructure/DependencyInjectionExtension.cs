using FluentMigrator.Runner;
using Gafel.Domain.Repositories;
using Gafel.Domain.Repositories.BankAccount;
using Gafel.Domain.Repositories.Category;
using Gafel.Domain.Repositories.Person;
using Gafel.Domain.Security.Tokens;
using Gafel.Domain.Services.CurrentUser;
using Gafel.Domain.Services.Identity;
using Gafel.Infrastructure.DataAccess;
using Gafel.Infrastructure.DataAccess.Repositories;
using Gafel.Infrastructure.Extensions;
using Gafel.Infrastructure.Security.Tokens.Access;
using Gafel.Infrastructure.Services.CurrentUser;
using Gafel.Infrastructure.Services.Identity.Configuration;
using Gafel.Infrastructure.Services.Identity.Entities;
using Gafel.Infrastructure.Services.Identity.Services;
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
        AddTokens(services, configuration);
        AddCurrentUser(services);
        AddIdentity(services);
        AddIdentityServices(services);
        AddRepositories(services);


        // Não precisa adicionar o contexto e nem o FluentMigrator nos testes
        if (configuration.IsUnitTestEnvironment())
            return;

        AddDbContextSqlSqerver(services, configuration);
        AddFluentMigratorSqlServer(services, configuration);
    }

    private static void AddTokens(IServiceCollection services, IConfiguration configuration)
    {
        var expirationTimeMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpirationTimeMinutes");
        var signinKey = configuration.GetValue<string>("Settings:Jwt:SigninKey");

        services.AddScoped<IAccessTokenGenerator>(option => new JwtTokenGenerator(expirationTimeMinutes, signinKey!));
    }
    private static void AddCurrentUser(IServiceCollection services) => services.AddScoped<ICurrentUser, CurrentUser>();

    private static void AddIdentity(IServiceCollection services)
    {
        // Identity 
        services
            .AddIdentityCore<ApplicationUser>(options =>
            {
                // Comprexidade da senha
                options.Password.RequiredLength = 8; // Comprimento mínimo da senha
                options.Password.RequireDigit = true; // Pelo menos um número
                options.Password.RequireNonAlphanumeric = true; // Pelo menos um caractere não alfanumérico (especial)
                options.Password.RequireLowercase = false; // Pelo menos uma letra minúscula
                options.Password.RequireUppercase = false; // Pelo menos uma letra maiúscula
                options.User.RequireUniqueEmail = true;
            })
            .AddRoles<IdentityRole<long>>()
            .AddRoleManager<RoleManager<IdentityRole<long>>>()
            .AddSignInManager()
            .AddEntityFrameworkStores<GafelDbContext>()
            .AddErrorDescriber<CustomIdentityErrorDescriber>()
            .AddDefaultTokenProviders();
    }
    private static void AddIdentityServices(IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserWriteOnlyService, IdentityUserService>();
        services.AddScoped<IUserReadOnlyService, IdentityUserService>();
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IPersonReadOnlyRepository, PersonRepository>();
        services.AddScoped<IPersonWriteOnlyRepository, PersonRepository>();
        services.AddScoped<IPersonUpdateOnlyRepository, PersonRepository>();

        services.AddScoped<ICategoryReadOnlyRepository, CategoryRepository>();
        services.AddScoped<ICategoryWriteOnlyRepository, CategoryRepository>();
        services.AddScoped<ICategoryUpdateOnlyRepository, CategoryRepository>();

        services.AddScoped<IBankAccountWriteOnlyRepository, BankAccountRepository>();
    }


    private static void AddDbContextSqlSqerver(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.ConnectionString();
        services.AddDbContext<GafelDbContext>(options => options.UseSqlServer(connectionString).UseSnakeCaseNamingConvention());
    }

    // Método responsável por configurar o FluentMigrator para utilizar SqlServer
    private static void AddFluentMigratorSqlServer(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.ConnectionString();

        // Registra os serviços principais do FluentMigrator no container de injeção de dependência
        services.AddFluentMigratorCore().ConfigureRunner(options =>
        {
            options.AddSqlServer()
            .WithGlobalConnectionString(connectionString) // Define a string de conexão que será usada pelas migrations

            // Define o assembly onde estão localizadas as classes de migration
            // Aqui ele carrega dinamicamente o assembly chamado "Gafel.Infrastructure"
            // e escaneia todas as classes que implementam migrations
            .ScanIn(Assembly.Load("Gafel.Infrastructure")).For.All();
        });
    }
}
