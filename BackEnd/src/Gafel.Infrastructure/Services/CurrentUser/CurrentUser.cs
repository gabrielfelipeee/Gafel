using Gafel.Domain.Constants;
using Gafel.Domain.Dtos;
using Gafel.Domain.Services.CurrentUser;
using Microsoft.AspNetCore.Http;

namespace Gafel.Infrastructure.Services.CurrentUser;

public class CurrentUser(IHttpContextAccessor contextAccessor) : ICurrentUser
{
    private readonly IHttpContextAccessor _contextAccessor = contextAccessor;

    UserDto ICurrentUser.CurrentUser()
    {
        return (UserDto)_contextAccessor.HttpContext.Items[HttpContextKeys.CurrentUser]!;
    }
}
