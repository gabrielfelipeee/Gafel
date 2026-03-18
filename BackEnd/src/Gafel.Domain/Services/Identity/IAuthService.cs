using Gafel.Domain.Dtos;

namespace Gafel.Domain.Services.Identity;

public interface IAuthService
{
    Task<LoginResponseDto> Login(UserCredentialsDto credentials);
}
