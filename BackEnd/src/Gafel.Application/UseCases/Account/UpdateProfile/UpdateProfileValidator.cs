using FluentValidation;
using Gafel.Domain.Enums;
using Gafel.Domain.Resources;

namespace Gafel.Application.UseCases.Account.UpdateProfile;

public class UpdateProfileValidator : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileValidator()
    {
        RuleFor(person => person.FullName)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.PERSON_FULL_NAME_REQUIRED)
            .MaximumLength(60)
            .WithMessage(ResourceMessagesException.PERSON_FULL_NAME_TOO_LONG);

        RuleFor(person => person.Cpf)
            .Length(11, 14)
            .WithMessage(ResourceMessagesException.PERSON_CPF_INVALID)
            .When(person => person.Cpf is not null);

        RuleFor(person => person.Uf)
            .Must(uf => Enum.TryParse<Uf>(uf, true, out _))
            .WithMessage(ResourceMessagesException.PERSON_UF_INVALID)
            .When(person => person.Uf is not null);

        RuleFor(person => person.City)
            .Length(3, 60)
            .WithMessage(ResourceMessagesException.PERSON_CITY_INVALID_LENGTH)
            .When(person => person.City is not null);
    }
}
