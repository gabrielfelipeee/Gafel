using Microsoft.AspNetCore.Identity;

namespace Gafel.Infrastructure.Services.Identity.Entities;

public class ApplicationUser : IdentityUser<long>
{
    public ApplicationUser(string email)
    {
        Email = email;
        UserName = email;
    }
}
