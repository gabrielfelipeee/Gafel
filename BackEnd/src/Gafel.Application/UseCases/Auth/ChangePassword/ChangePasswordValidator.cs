using FluentValidation;
using Gafel.Domain.Resources;
using Gafel.Application.RuleExtensions;

namespace Gafel.Application.UseCases.Auth.ChangePassword;

public class ChangePasswordValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordValidator()
    {
        RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.USER_PASSWORD_EMPTY);
        RuleFor(x => x.Password)
            .PasswordPolicy()
            .When(x => !string.IsNullOrEmpty(x.Password));

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.USER_PASSWORD_EMPTY);
        RuleFor(x => x.NewPassword)
            .PasswordPolicy()
            .When(x => !string.IsNullOrEmpty(x.NewPassword));
    }
}
