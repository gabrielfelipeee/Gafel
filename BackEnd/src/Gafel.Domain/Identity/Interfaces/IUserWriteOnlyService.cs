using Gafel.Domain.Identity.Dtos;

namespace Gafel.Domain.Identity.Interfaces;

public interface IUserWriteOnlyService
{
    Task<RegisterUserResult> Register(RegisterUserRequest request);
    //   Task<(bool Success, Dictionary<string, string[]> Errors)> Update(long userId, string email);
}
