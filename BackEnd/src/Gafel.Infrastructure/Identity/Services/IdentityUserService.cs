using Gafel.Domain.Constants;
using Gafel.Domain.Identity.Dtos;
using Gafel.Domain.Identity.Interfaces;
using Gafel.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace Gafel.Infrastructure.Identity.Services;

public class IdentityUserService(UserManager<ApplicationUser> userManager) : IUserWriteOnlyService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<RegisterUserResult> Register(RegisterUserRequest request)
    {
        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
            return new RegisterUserResult(Success: false, Errors: MapErrors(result.Errors));

        await _userManager.AddToRoleAsync(user, Roles.User);

        return new RegisterUserResult(Success: true, UserId: user.Id);
    }

    /*
    public async Task<(bool Success, IEnumerable<string> Errors)> Update(string identityUserId, string newEmail)
    {
        var user = await _userManager.FindByIdAsync(identityUserId);
        if (user == null)
            return (false, new[] { "Usuário não encontrado" });

        // No ASP.NET Identity, mudar e-mail e username exige cuidado com a normalização
        var result = await _userManager.SetEmailAsync(user, newEmail);
        if (!result.Succeeded)
            return (false, result.Errors.Select(e => e.Description));

        // Frequentemente em apps de finanças, o Login (UserName) é o próprio Email
        var resultUsername = await _userManager.SetUserNameAsync(user, newEmail);

        return (resultUsername.Succeeded, resultUsername.Errors.Select(e => e.Description));
    }
    */

    private static Dictionary<string, string[]> MapErrors(IEnumerable<IdentityError> errors)
    {
        return errors
            .GroupBy(e => e.Code)
            .ToDictionary(
                g => g.Key,
                g => g.DistinctBy(x => x.Description).Select(e => e.Description).ToArray());
    }
}
