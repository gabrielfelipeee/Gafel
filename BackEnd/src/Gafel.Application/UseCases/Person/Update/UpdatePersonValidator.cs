using FluentValidation;
using Gafel.Domain.Enums;
using Gafel.Domain.Resources;

namespace Gafel.Application.UseCases.Person.Update;

public class UpdatePersonValidator : AbstractValidator<UpdatePersonCommand>
{
    public UpdatePersonValidator()
    {
        RuleFor(person => person.FullName)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.PERSON_FULL_NAME_EMPTY);

        RuleFor(person => person.Cpf)
            .Length(11, 14)
            .WithMessage(ResourceMessagesException.CPF_INVALID)
            .When(x => !string.IsNullOrWhiteSpace(x.Cpf));

        RuleFor(person => person.Uf)
            .Must(uf => Enum.TryParse<Uf>(uf, true, out _))
            .WithMessage(ResourceMessagesException.PERSON_UF_INVALID)
            .When(person => !string.IsNullOrWhiteSpace(person.Uf));

        RuleFor(person => person.City)
            .MinimumLength(2)
            .MaximumLength(60)
            .WithMessage(ResourceMessagesException.PERSON_CITY_INVALID)
            .When(person => !string.IsNullOrWhiteSpace(person.City));
    }
}
