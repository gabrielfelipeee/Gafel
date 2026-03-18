using FluentValidation;
using Gafel.Domain.Resources;
using Gafel.Application.RuleExtensions;

namespace Gafel.Application.UseCases.Auth.Login.DoLogin;

public class DoLoginValidator : AbstractValidator<DoLoginCommand>
{
    public DoLoginValidator()
    {
        RuleFor(user => user.Email)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.USER_EMAIL_EMPTY);

        RuleFor(user => user.Email)
            .EmailAddress()
            .WithMessage(ResourceMessagesException.USER_EMAIL_INVALID)
            .When(user => !string.IsNullOrEmpty(user.Email));


        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.USER_PASSWORD_EMPTY);

        RuleFor(x => x.Password)
            .PasswordPolicy()
            .When(x => !string.IsNullOrEmpty(x.Password));
    }
}
