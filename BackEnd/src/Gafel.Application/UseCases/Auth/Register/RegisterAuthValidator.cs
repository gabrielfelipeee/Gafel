using FluentValidation;
using Gafel.Domain.Resources;
using Gafel.Application.RuleExtensions;

namespace Gafel.Application.UseCases.Auth.Register;

public class RegisterAuthValidator : AbstractValidator<RegisterAuthCommand>
{
    public RegisterAuthValidator()
    {
        RuleFor(person => person.FullName)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.NAME_EMPTY);

        RuleFor(user => user.Email)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.EMAIL_EMPTY);

        // Para evitar que o a mensagem de email inválido seja enviado quando o email for nulo ou vazio
        When(user => !string.IsNullOrEmpty(user.Email), () =>
        {
            RuleFor(user => user.Email)
                .EmailAddress()
                .WithMessage(ResourceMessagesException.EMAIL_INVALID);
        });
        
        RuleFor(user => user.Password).PasswordRequirements();
    }
}
