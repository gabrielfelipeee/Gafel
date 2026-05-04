using Gafel.Domain.Dtos;
using Gafel.Domain.Dtos.Responses;

namespace Gafel.Domain.Services.Identity;

public interface IAuthService
{
    Task<LoginResponseDto> Login(UserCredentialsDto credentials);
    Task<ChangePasswordResponseDto> ChangePassword(long userId, string password, string newPassword);
}
