using Gafel.Domain.Resources;
using Microsoft.AspNetCore.Identity;

namespace Gafel.Infrastructure.Services.Identity.Configuration;

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
            Description = ResourceMessagesException.USER_EMAIL_ALREADY_REGISTERED
        };
    }
    public override IdentityError InvalidEmail(string? email)
    {
        return new IdentityError
        {
            Code = EmailField,
            Description = ResourceMessagesException.USER_EMAIL_INVALID
        };
    }

    public override IdentityError DuplicateUserName(string userName)
    {
        return new IdentityError
        {
            Code = EmailField,
            Description = ResourceMessagesException.USER_EMAIL_ALREADY_REGISTERED
        };
    }
    #endregion

    #region Password
    public override IdentityError PasswordTooShort(int length)
    {
        return new IdentityError
        {
            Code = PasswordField,
            Description = ResourceMessagesException.USER_PASSWORD_TOO_SHORT
        };
    }

    public override IdentityError PasswordRequiresNonAlphanumeric()
    {
        return new IdentityError
        {
            Code = PasswordField,
            Description = ResourceMessagesException.USER_PASSWORD_REQUIRES_SPECIAL_CHAR
        };
    }

    public override IdentityError PasswordRequiresDigit()
    {
        return new IdentityError
        {
            Code = PasswordField,
            Description = ResourceMessagesException.USER_PASSWORD_REQUIRES_NUMBER
        };
    }

    public override IdentityError PasswordMismatch()
    {
        return new IdentityError
        {
            Code = PasswordField,
            Description = ResourceMessagesException.USER_PASSWORD_INCORRECT
        };
    }
    #endregion
}
