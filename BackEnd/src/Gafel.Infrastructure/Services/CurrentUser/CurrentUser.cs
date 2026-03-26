using Gafel.Domain.Services.CurrentUser;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Gafel.Infrastructure.Services.CurrentUser;

public class CurrentUser(IHttpContextAccessor contextAccessor) : ICurrentUser
{
    private readonly IHttpContextAccessor _contextAccessor = contextAccessor;

    public long UserId
    {
        get
        {
            var value = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Sid)!;
            return long.Parse(value);
        }
    }
}
