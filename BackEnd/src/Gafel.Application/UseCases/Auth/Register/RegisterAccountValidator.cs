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
            .WithMessage(ResourceMessagesException.FULL_NAME_EMPTY);


        RuleFor(user => user.Email)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.EMAIL_EMPTY);

        RuleFor(user => user.Email)
            .EmailAddress()
            .WithMessage(ResourceMessagesException.EMAIL_INVALID)
            .When(user => !string.IsNullOrEmpty(user.Email));


        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.PASSWORD_EMPTY);
        RuleFor(x => x.Password)
            .PasswordPolicy()
            .When(x => !string.IsNullOrEmpty(x.Password));
    }
}
