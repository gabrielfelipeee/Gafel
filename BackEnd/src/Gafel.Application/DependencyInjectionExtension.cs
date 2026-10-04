using Gafel.Application.Services.Mapster;
using Gafel.Application.UseCases.Account.GetProfile;
using Gafel.Application.UseCases.Account.UpdateProfile;
using Gafel.Application.UseCases.Auth.ChangePassword;
using Gafel.Application.UseCases.Auth.Login.DoLogin;
using Gafel.Application.UseCases.Auth.Register;
using Gafel.Application.UseCases.BankAccount.Create;
using Gafel.Application.UseCases.BankAccount.Delete;
using Gafel.Application.UseCases.BankAccount.Filter;
using Gafel.Application.UseCases.BankAccount.GetById;
using Gafel.Application.UseCases.BankAccount.Update;
using Gafel.Application.UseCases.Category.Create;
using Gafel.Application.UseCases.Category.Delete;
using Gafel.Application.UseCases.Category.Filter;
using Gafel.Application.UseCases.Category.GetById;
using Gafel.Application.UseCases.Category.Update;
using Gafel.Application.UseCases.Transaction.Create;
using Gafel.Application.UseCases.Transaction.Update;
using Microsoft.Extensions.DependencyInjection;

namespace Gafel.Application;

public static class DependencyInjectionExtension
{
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

        services.AddScoped<IGetCategoryByIdUseCase, GetCategoryByIdUseCase>();
        services.AddScoped<IFilterCategoryUseCase, FilterCategoryUseCase>();
        services.AddScoped<ICreateCategoryUseCase, CreateCategoryUseCase>();
        services.AddScoped<IUpdateCategoryUseCase, UpdateCategoryUseCase>();
        services.AddScoped<IDeleteCategoryUseCase, DeleteCategoryUseCase>();

        services.AddScoped<IGetBankAccountByIdUseCase, GetBankAccountByIdUseCase>();
        services.AddScoped<IFilterBankAccountUseCase, FilterBankAccountUseCase>();
        services.AddScoped<ICreateBankAccountUseCase, CreateBankAccountUseCase>();
        services.AddScoped<IUpdateBankAccountUseCase, UpdateBankAccountUseCase>();
        services.AddScoped<IDeleteBankAccountUseCase, DeleteBankAccountUseCase>();

        services.AddScoped<ICreateTransactionUseCase, CreateTransactionUseCase>();
        services.AddScoped<IUpdateTransactionUseCase, UpdateTransactionUseCase>();
    }

    private static void AddMapsterConfigurations() => MapsterConfigurations.Configure();
}
