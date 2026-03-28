using CommonTestUtilities.Commands;
using Gafel.Domain.Constants;
using Gafel.Domain.Entities;
using Gafel.Infrastructure.DataAccess;
using Gafel.Infrastructure.Services.Identity.Entities;
using Mapster;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace WebAPI.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private string _personFullName = default!;
    private string _userEmail = default!;
    private string _userPassword = default!;
    private long _userId = default!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test")
            .ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<GafelDbContext>));
                if (descriptor is not null)
                    services.Remove(descriptor);

                var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();
                services.AddDbContext<GafelDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                    options.UseInternalServiceProvider(provider);

                    options.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
                });

                using var scope = services.BuildServiceProvider().CreateScope();

                var context = scope.ServiceProvider.GetRequiredService<GafelDbContext>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<long>>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                context.Database.EnsureDeleted(); // Garante que o db inicie vazio

                StartDatabase(context, userManager, roleManager).GetAwaiter().GetResult();
            });
    }

    public string GetPersonFullName() => _personFullName;

    public long GetUserId() => _userId;
    public string GetUserEmail() => _userEmail;
    public string GetUserPassword() => _userPassword;


    private async Task StartDatabase(GafelDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<long>> roleManager)
    {
        foreach (var role in Roles.All)
            await roleManager.CreateAsync(new IdentityRole<long>(role));

        var commandRegister = RegisterAccountCommandBuilder.Build();
        _personFullName = commandRegister.FullName;
        _userEmail = commandRegister.Email;
        _userPassword = commandRegister.Password;

        var user = new ApplicationUser(commandRegister.Email);
        await userManager.CreateAsync(user, commandRegister.Password);
        _userId = user.Id;

        var person = commandRegister.Adapt<Person>();
        person.UserId = user.Id;
        context.People.Add(person);

        context.SaveChanges();
    }
}
