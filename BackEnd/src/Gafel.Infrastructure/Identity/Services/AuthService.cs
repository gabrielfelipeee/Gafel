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

    public async Task<LoginResult> Login(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return new LoginResult(Success: false, ErrorMessage: ResourceMessagesException.INVALID_CREDENTIALS);

        var result = await _signInManager.CheckPasswordSignInAsync(user: user, password: request.Password, lockoutOnFailure: true);
        if (!result.Succeeded)
        {
            var message = result switch
            {
                { IsLockedOut: true } => ResourceMessagesException.LOCKED_OUT,
                { IsNotAllowed: true } => ResourceMessagesException.LOGIN_NOT_ALLOWED,
                { RequiresTwoFactor: true } => ResourceMessagesException.TWO_FACTOR_REQUIRED,
                _ => ResourceMessagesException.INVALID_CREDENTIALS
            };

            return new LoginResult(Success: false, ErrorMessage: message);
        }

        return new LoginResult(Success: true, UserId: user.Id);
    }
}
