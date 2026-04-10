using FluentValidation;
using Gafel.Domain.Resources;
using Gafel.Application.RuleExtensions;

namespace Gafel.Application.UseCases.Auth.Register;

public class RegisterAccountValidator : AbstractValidator<RegisterAccountCommand>
{
    public RegisterAccountValidator()
    {
        RuleFor(person => person.FullName)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.PERSON_FULL_NAME_EMPTY);

        RuleFor(user => user.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.USER_EMAIL_EMPTY)
            .EmailAddress()
            .WithMessage(ResourceMessagesException.USER_EMAIL_INVALID);

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.USER_PASSWORD_EMPTY)
            .DependentRules(() => RuleFor(x => x.Password).PasswordPolicy());
    }
}
