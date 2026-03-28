using Gafel.Domain.Dtos;

namespace Gafel.Domain.Services.Identity;

public interface IUserReadOnlyService
{
    Task<UserDto?> GetById(long userId);
}
