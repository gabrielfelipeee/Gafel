using FluentValidation;
using Gafel.Domain.Resources;
using Gafel.Application.UseCases.Auth.Shared.Extensions;

namespace Gafel.Application.UseCases.Auth.Register;

public class RegisterAccountValidator : AbstractValidator<RegisterAccountCommand>
{
    public RegisterAccountValidator()
    {
        RuleFor(person => person.FullName)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.PERSON_FULL_NAME_REQUIRED)
            .MaximumLength(60)
            .WithMessage(ResourceMessagesException.PERSON_FULL_NAME_TOO_LONG);

        RuleFor(user => user.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.USER_EMAIL_REQUIRED)
            .EmailAddress()
            .WithMessage(ResourceMessagesException.USER_EMAIL_INVALID);

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.USER_PASSWORD_REQUIRED)
            .DependentRules(() => RuleFor(x => x.Password).PasswordPolicy());
    }
}
