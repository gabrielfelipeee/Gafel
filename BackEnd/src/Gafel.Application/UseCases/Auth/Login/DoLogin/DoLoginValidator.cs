using FluentValidation;
using Gafel.Domain.Resources;
using Gafel.Application.UseCases.Auth.Shared.Extensions;

namespace Gafel.Application.UseCases.Auth.Login.DoLogin;

public class DoLoginValidator : AbstractValidator<DoLoginCommand>
{
    public DoLoginValidator()
    {
        RuleFor(user => user.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.USER_EMAIL_REQUIRED)
            .EmailAddress()
            .WithMessage(ResourceMessagesException.USER_EMAIL_INVALID);

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.USER_PASSWORD_REQUIRED)
            .DependentRules(() => RuleFor(x => x.Password).PasswordPolicy()); // Só será executada se a regra anterior (NotEmpty) for válida.
    }
}
