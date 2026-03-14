using Gafel.Domain.Identity.Dtos;

namespace Gafel.Domain.Identity.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto> Login(UserCredentialsDto credentials);
}
