using Gafel.Application.Services.Mapster;
using Gafel.Application.UseCases.Account.GetProfile;
using Gafel.Application.UseCases.Account.UpdateProfile;
using Gafel.Application.UseCases.Auth.ChangePassword;
using Gafel.Application.UseCases.Auth.Login.DoLogin;
using Gafel.Application.UseCases.Auth.Register;
using Gafel.Application.UseCases.Category.Register;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Gafel.Application;

public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        AddUseCases(services);
        AddMapsterConfigurations();
      //  AddIdObfuscation(services, configuration);
    }

    private static void AddUseCases(IServiceCollection services)
    {
        services.AddScoped<IRegisterAccountUseCase, RegisterAccountUseCase>();
        services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
        services.AddScoped<IChangePasswordUseCase, ChangePasswordUseCase>();

        services.AddScoped<IUpdateProfileUseCase, UpdateProfileUseCase>();
        services.AddScoped<IGetProfileUseCase, GetProfileUseCase>();

        services.AddScoped<IRegisterCategoryUseCase, RegisterCategoryUseCase>();
    }

    private static void AddMapsterConfigurations()
    {
        MapsterConfigurations.Configure();
    }

    /*
    private static void AddMapsterConfigurations(IServiceCollection services)
    {
        services.AddSingleton(provider =>
        {
            var sqids = provider.GetRequiredService<SqidsEncoder<long>>();

            return new MapsterConfigurations(sqids);
        });
    }
    */
    /*
    private static void AddIdObfuscation(IServiceCollection services, IConfiguration configuration)
    {
        var sqids = new SqidsEncoder<long>(new()
        {
            Alphabet = configuration.GetValue<string>("Settings:IdObfuscation:Alphabet")!,
            MinLength = configuration.GetValue<int>("Settings:IdObfuscation:MinimumLength")
        });
        services.AddSingleton(sqids);
    }
    */
}
