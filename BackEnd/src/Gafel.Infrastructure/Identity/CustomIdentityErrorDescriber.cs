using Gafel.Domain.Resources;
using Microsoft.AspNetCore.Identity;

namespace Gafel.Infrastructure.Identity;

public class CustomIdentityErrorDescriber : IdentityErrorDescriber
{
    private const string EmailField = "Email";
    private const string PasswordField = "Password";


    #region Email and UserName
    public override IdentityError DuplicateEmail(string email)
    {
        return new IdentityError
        {
            Code = EmailField,
            Description = ResourceMessagesException.EMAIL_ALREADY_REGISTERED
        };
    }
    public override IdentityError InvalidEmail(string? email)
    {
        return new IdentityError
        {
            Code = EmailField,
            Description = ResourceMessagesException.EMAIL_INVALID
        };
    }

    public override IdentityError DuplicateUserName(string userName)
    {
        return new IdentityError
        {
            Code = EmailField,
            Description = ResourceMessagesException.EMAIL_ALREADY_REGISTERED
        };
    }
    #endregion

    #region Password
    public override IdentityError PasswordTooShort(int length)
    {
        return new IdentityError
        {
            Code = PasswordField,
            Description = ResourceMessagesException.PASSWORD_TOO_SHORT
        };
    }

    public override IdentityError PasswordRequiresNonAlphanumeric()
    {
        return new IdentityError
        {
            Code = PasswordField,
            Description = ResourceMessagesException.PASSWORD_REQUIRES_SPECIAL_CHARACTER
        };
    }

    public override IdentityError PasswordRequiresDigit()
    {
        return new IdentityError
        {
            Code = PasswordField,
            Description = ResourceMessagesException.PASSWORD_REQUIRES_NUMBER
        };
    }
    #endregion
}
