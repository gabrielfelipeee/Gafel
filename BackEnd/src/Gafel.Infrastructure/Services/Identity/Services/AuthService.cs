using Gafel.Domain.Dtos;
using Gafel.Domain.Resources;
using Gafel.Domain.Services.Identity;
using Gafel.Infrastructure.Extensions;
using Gafel.Infrastructure.Services.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace Gafel.Infrastructure.Services.Identity.Services;

public class AuthService(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager) : IAuthService
{
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<LoginResponseDto> Login(UserCredentialsDto credentials)
    {
        var user = await _userManager.FindByEmailAsync(credentials.Email);
        if (user is null)
            return new LoginResponseDto(Success: false, ErrorMessage: ResourceMessagesException.AUTH_INVALID_CREDENTIALS);

        var result = await _signInManager.CheckPasswordSignInAsync(user: user, password: credentials.Password, lockoutOnFailure: true);
        if (!result.Succeeded)
        {
            var message = result switch
            {
                { IsLockedOut: true } => ResourceMessagesException.AUTH_LOCKED_OUT,
                { IsNotAllowed: true } => ResourceMessagesException.AUTH_LOGIN_NOT_ALLOWED,
                { RequiresTwoFactor: true } => ResourceMessagesException.AUTH_TWO_FACTOR_REQUIRED,
                _ => ResourceMessagesException.AUTH_INVALID_CREDENTIALS
            };

            return new LoginResponseDto(Success: false, ErrorMessage: message);
        }

        return new LoginResponseDto(Success: true, UserId: user.Id);
    }

    public async Task<ChangePasswordResponseDto> ChangePassword(long userId, string password, string newPassword)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        var result = await _userManager.ChangePasswordAsync(user!, password, newPassword);
        if (!result.Succeeded)
            return new ChangePasswordResponseDto(Success: false, Errors: result.Errors.ToErrorsByCodeDictionary());

        return new ChangePasswordResponseDto(Success: result.Succeeded);
    }
}
