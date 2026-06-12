using Gafel.Application.Services.Mapster;
using Gafel.Application.UseCases.Account.GetProfile;
using Gafel.Application.UseCases.Account.UpdateProfile;
using Gafel.Application.UseCases.Auth.ChangePassword;
using Gafel.Application.UseCases.Auth.Login.DoLogin;
using Gafel.Application.UseCases.Auth.Register;
using Gafel.Application.UseCases.Category.Delete;
using Gafel.Application.UseCases.Category.Filter;
using Gafel.Application.UseCases.Category.GetById;
using Gafel.Application.UseCases.Category.Register;
using Gafel.Application.UseCases.Category.Update;
using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sqids;

namespace Gafel.Application;

public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        AddUseCases(services);

        AddIdObfuscation(services, configuration, out SqidsEncoder<long> sqids);
        AddMapsterConfigurations(sqids);
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
        services.AddScoped<IDeleteCategoryUseCase, DeleteCategoryUseCase>();
    }

    private static void AddIdObfuscation(IServiceCollection services, IConfiguration configuration, out SqidsEncoder<long> sqids)
    {
        sqids = new SqidsEncoder<long>(new()
        {
            Alphabet = configuration.GetValue<string>("Settings:IdObfuscation:Alphabet")!,
            MinLength = configuration.GetValue<int>("Settings:IdObfuscation:MinimumLength")
        });
        services.AddSingleton(sqids);
    }
    private static void AddMapsterConfigurations(SqidsEncoder<long> sqids)
    {
        var config = TypeAdapterConfig.GlobalSettings;

        MapsterConfigurations.Configure(config, sqids);
    }
}
