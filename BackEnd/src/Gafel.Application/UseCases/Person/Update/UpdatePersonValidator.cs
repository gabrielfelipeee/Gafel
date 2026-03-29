using FluentValidation;
using Gafel.Domain.Constants;
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
            .IsValidCPF()
            .WithMessage(ResourceMessagesException.PERSON_CPF_INVALID)
            .When(person => !string.IsNullOrWhiteSpace(person.Cpf));

        RuleFor(person => person.DateOfBirth)
            .Custom((dateOfBirth, context) =>
            {
                var today = DateOnly.FromDateTime(DateTime.UtcNow);

                if (dateOfBirth > today)
                    context.AddFailure(ResourceMessagesException.PERSON_DATE_OF_BIRTH_FUTURE);

                if (dateOfBirth > today.AddYears(-(int)GafelRuleContants.MINIMUM_AGE))
                    context.AddFailure(ResourceMessagesException.PERSON_DATE_OF_BIRTH_INVALID);
            })
            .When(person => person.DateOfBirth.HasValue);

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
