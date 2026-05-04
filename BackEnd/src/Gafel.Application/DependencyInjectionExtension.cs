using Gafel.Application.Services.Mapster;
using Gafel.Application.UseCases.Account.GetProfile;
using Gafel.Application.UseCases.Account.UpdateProfile;
using Gafel.Application.UseCases.Auth.ChangePassword;
using Gafel.Application.UseCases.Auth.Login.DoLogin;
using Gafel.Application.UseCases.Auth.Register;
using Gafel.Application.UseCases.Category.Filter;
using Gafel.Application.UseCases.Category.GetById;
using Gafel.Application.UseCases.Category.Register;
using Gafel.Application.UseCases.Category.Update;
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

        services.AddScoped<IGetCategoryByIdUseCase, GetCategoryByIdUseCase>();
        services.AddScoped<IFilterCategoryUseCase, FilterCategoryUseCase>();
        services.AddScoped<IRegisterCategoryUseCase, RegisterCategoryUseCase>();
        services.AddScoped<IUpdateCategoryUseCase, UpdateCategoryUseCase>();
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
