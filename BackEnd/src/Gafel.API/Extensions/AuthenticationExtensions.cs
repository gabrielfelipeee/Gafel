using Gafel.Application.Exceptions;
using Gafel.Domain.Constants;
using Gafel.Domain.Services.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Gafel.API.Extensions;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var signinKey = configuration.GetValue<string>("Settings:Jwt:SigninKey");

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = CreateTokenParameters(signinKey!);
            options.Events = CreateJwtEvents();
        });

        return services;
    }

    private static TokenValidationParameters CreateTokenParameters(string signinKey) => new()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signinKey)),
        RequireExpirationTime = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };

    private static JwtBearerEvents CreateJwtEvents() => new()
    {
        OnAuthenticationFailed = context =>
        {
            context.HttpContext.Items[HttpContextKeys.AuthException] = context.Exception;
            return Task.CompletedTask;
        },

        OnTokenValidated = async context => await ValidateUserExistence(context)
    };

    private static async Task ValidateUserExistence(TokenValidatedContext context)
    {
        var claimValue = context.Principal?.FindFirst(ClaimTypes.Sid)?.Value;
        if (!long.TryParse(claimValue, out var userId))
        {
            context.Fail(string.Empty);
            return;
        }

        var userReadIOnlyService = context.HttpContext.RequestServices.GetRequiredService<IUserReadOnlyService>();

        var user = await userReadIOnlyService.GetById(userId);

        if (user is null)
        {
            context.HttpContext.Items[HttpContextKeys.AuthException] = new InvalidUserTokenException();
            context.Fail(string.Empty);
            return;
        }

        context.HttpContext.Items[HttpContextKeys.CurrentUser] = user;
    }
}
