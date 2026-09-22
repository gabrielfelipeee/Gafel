using Microsoft.AspNetCore.Identity;

namespace Gafel.Infrastructure.Services.Identity.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public ApplicationUser(string email)
    {
        Id = Guid.CreateVersion7();
        Email = email;
        UserName = email;
    }
}
