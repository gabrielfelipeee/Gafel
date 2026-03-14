using Gafel.Domain.Identity.Dtos;

namespace Gafel.Domain.Identity.Interfaces;

public interface IUserWriteOnlyService
{
    Task<RegisterUserResponseDto> Register(UserCredentialsDto request);
    //   Task<(bool Success, Dictionary<string, string[]> Errors)> Update(long userId, string email);
}
