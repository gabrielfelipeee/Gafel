using Gafel.Domain.Dtos;

namespace Gafel.Domain.Services.Identity;

public interface IUserWriteOnlyService
{
    Task<RegisterUserResponseDto> Register(UserCredentialsDto request);
    //   Task<(bool Success, Dictionary<string, string[]> Errors)> Update(long userId, string email);
}
