using Gafel.Domain.Constants;
using Gafel.Domain.Dtos;
using Gafel.Domain.Services.Identity;
using Gafel.Infrastructure.Extensions;
using Gafel.Infrastructure.Services.Identity.Entities;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Gafel.Infrastructure.Services.Identity.Services;

public class IdentityUserService(UserManager<ApplicationUser> userManager) : IUserReadOnlyService, IUserWriteOnlyService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<UserDto?> GetById(long userId)
    {
        return await _userManager.Users
            .AsNoTracking()
            .Where(user => user.Id == userId)
            .ProjectToType<UserDto>()
            .FirstOrDefaultAsync();
    }

    public async Task<RegisterUserResponseDto> Register(UserCredentialsDto request)
    {
        var user = new ApplicationUser(request.Email);

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
            return new RegisterUserResponseDto(Success: false, Errors: result.Errors.ToErrorsByCodeDictionary());

        await _userManager.AddToRoleAsync(user, Roles.User);

        return new RegisterUserResponseDto(Success: true, UserId: user.Id);
    }
}
