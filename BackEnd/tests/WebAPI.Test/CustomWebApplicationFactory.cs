using CommonTestUtilities.Dtos;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Helpers;
using Gafel.Domain.Constants;
using Gafel.Infrastructure.DataAccess;
using Gafel.Infrastructure.Services.Identity.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Bogus;

namespace WebAPI.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private string _personFullName = default!;
    private string _personCpf = default!;
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
    public string GetPersonCpf() => _personCpf;

    public long GetUserId() => _userId;
    public string GetUserEmail() => _userEmail;
    public string GetUserPassword() => _userPassword;


    private async Task StartDatabase(GafelDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<long>> roleManager)
    {
        foreach (var role in Roles.All)
            await roleManager.CreateAsync(new IdentityRole<long>(role));

        var userDto = UserDtoBuilder.Build();
        var password = PasswordGenerator.Generate(new Faker());

        var applicationUser = new ApplicationUser(userDto.Email);
        await userManager.CreateAsync(applicationUser, password);


        var person = PersonBuilder.Build(withCpf: true, withDateOfBirth: true);
        person.UserId = userDto.Id;
        context.People.Add(person);

        context.SaveChanges();

        _userId = applicationUser.Id;
        _userEmail = userDto.Email;
        _userPassword = password;

        _personFullName = person.FullName;
        _personCpf = person.Cpf!.Value;
    }
}
