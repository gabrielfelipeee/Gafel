using FluentValidation;
using Gafel.Domain.Resources;
using Gafel.Application.UseCases.Auth.Shared.Extensions;

namespace Gafel.Application.UseCases.Auth.ChangePassword;

public class ChangePasswordValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordValidator()
    {
        RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.USER_PASSWORD_EMPTY)
                .DependentRules(() => RuleFor(x => x.Password).PasswordPolicy());

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.USER_PASSWORD_EMPTY)
            .DependentRules(() => RuleFor(x => x.NewPassword).PasswordPolicy());
    }
}
