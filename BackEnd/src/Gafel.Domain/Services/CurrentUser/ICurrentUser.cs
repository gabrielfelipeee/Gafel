using Gafel.Domain.Dtos;

namespace Gafel.Domain.Services.CurrentUser;

public interface ICurrentUser
{
    UserDto CurrentUser();
}
