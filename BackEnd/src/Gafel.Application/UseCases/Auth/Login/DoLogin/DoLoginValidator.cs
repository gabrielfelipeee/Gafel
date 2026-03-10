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
            .WithMessage(ResourceMessagesException.EMAIL_EMPTY);

        When(user => !string.IsNullOrEmpty(user.Email), () =>
        {
            RuleFor(user => user.Email)
                .EmailAddress()
                .WithMessage(ResourceMessagesException.EMAIL_INVALID);
        });

        RuleFor(user => user.Password).PasswordRequirements();
    }
}
