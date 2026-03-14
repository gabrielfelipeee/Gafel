using Gafel.Domain.Identity.Dtos;
using Gafel.Domain.Identity.Interfaces;
using Gafel.Domain.Resources;
using Gafel.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace Gafel.Infrastructure.Identity.Services;

public class AuthService(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager) : IAuthService
{
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<LoginResponseDto> Login(UserCredentialsDto credentials)
    {
        var user = await _userManager.FindByEmailAsync(credentials.Email);
        if (user is null)
            return new LoginResponseDto(Success: false, ErrorMessage: ResourceMessagesException.INVALID_CREDENTIALS);

        var result = await _signInManager.CheckPasswordSignInAsync(user: user, password: credentials.Password, lockoutOnFailure: true);
        if (!result.Succeeded)
        {
            var message = result switch
            {
                { IsLockedOut: true } => ResourceMessagesException.LOCKED_OUT,
                { IsNotAllowed: true } => ResourceMessagesException.LOGIN_NOT_ALLOWED,
                { RequiresTwoFactor: true } => ResourceMessagesException.TWO_FACTOR_REQUIRED,
                _ => ResourceMessagesException.INVALID_CREDENTIALS
            };

            return new LoginResponseDto(Success: false, ErrorMessage: message);
        }

        return new LoginResponseDto(Success: true, UserId: user.Id);
    }
}
