using Gafel.Domain.Dtos;
using Gafel.Domain.Dtos.Responses;

namespace Gafel.Domain.Services.Identity;

public interface IUserWriteOnlyService
{
    Task<RegisterUserResponseDto> Register(UserCredentialsDto request);
}
