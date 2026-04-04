using Gafel.Application.Services.Mapster;
using Gafel.Application.UseCases.Account.GetProfile;
using Gafel.Application.UseCases.Account.UpdateProfile;
using Gafel.Application.UseCases.Auth.ChangePassword;
using Gafel.Application.UseCases.Auth.Login.DoLogin;
using Gafel.Application.UseCases.Auth.Register;
using Microsoft.Extensions.DependencyInjection;

namespace Gafel.Application;

// Classe estática que vai conter o método de extensão. Em C#, métodos de extensão devem ser em classes estáticas.
public static class DependencyInjectionExtension
{
    // this está dizendo ao compilador que o método AddApplication é um método de extensão para a interface IServiceCollection.
    public static void AddApplication(this IServiceCollection services)
    {
        AddUseCases(services);
        AddMapsterConfigurations();
    }

    private static void AddUseCases(IServiceCollection services)
    {
        services.AddScoped<IRegisterAccountUseCase, RegisterAccountUseCase>();
        services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
        services.AddScoped<IChangePasswordUseCase, ChangePasswordUseCase>();

        services.AddScoped<IUpdateProfileUseCase, UpdateProfileUseCase>();
        services.AddScoped<IGetProfileUseCase, GetProfileUseCase>();
    }

    private static void AddMapsterConfigurations()
    {
        MapsterConfigurations.Configure();
    }
}
