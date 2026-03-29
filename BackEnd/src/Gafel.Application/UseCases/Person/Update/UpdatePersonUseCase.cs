using FluentValidation.Results;
using Gafel.Application.Exceptions;
using Gafel.Application.Extensions;
using Gafel.Domain.Repositories;
using Gafel.Domain.Repositories.Person;
using Gafel.Domain.Resources;
using Gafel.Domain.Services.CurrentUser;
using Mapster;

namespace Gafel.Application.UseCases.Person.Update;

public class UpdatePersonUseCase : IUpdatePersonUseCase
{
    private readonly ICurrentUser _currentUser;
    private readonly IPersonUpdateOnlyRepository _personUpdateOnlyRepository;
    private readonly IPersonReadOnlyRepository _personReadOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePersonUseCase(
        ICurrentUser currentUser,
        IPersonUpdateOnlyRepository personUpdateOnlyRepository,
        IPersonReadOnlyRepository personReadOnlyRepository,
        IUnitOfWork unitOfWork)
    {
        _currentUser = currentUser;
        _personUpdateOnlyRepository = personUpdateOnlyRepository;
        _personReadOnlyRepository = personReadOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(UpdatePersonCommand request)
    {
        var currentUser = _currentUser.CurrentUser();

        var person = await _personUpdateOnlyRepository.GetByUserId(currentUser.Id)
            ?? throw new PersonNotFoundException();

        await Validate(
            request: request,
            personId: person.Id,
            currentCpf: person.Cpf,
            currentDateOfBirth: person.DateOfBirth
        );

        request.Adapt(person);
        person.UpdatedAt = DateTime.UtcNow;

        _personUpdateOnlyRepository.Update(person);
        await _unitOfWork.SaveChangesAsync();
    }

    private async Task Validate(UpdatePersonCommand request, long personId, string? currentCpf = null, DateOnly? currentDateOfBirth = null)
    {
        var result = new UpdatePersonValidator().Validate(request);

        // Já tem data de nascimento, não pode ser atualizado
        if (currentDateOfBirth.HasValue && !currentDateOfBirth.Equals(request.DateOfBirth))
            result.Errors.Add(new ValidationFailure(nameof(request.DateOfBirth), ResourceMessagesException.PERSON_DATE_OF_BIRTH_UPDATE_NOT_ALLOWED));

        // Já tem CPF, não pode ser atualizado
        var hasCurrentCpf = !string.IsNullOrWhiteSpace(currentCpf);
        var hasNewCpf = !string.IsNullOrWhiteSpace(request.Cpf);

        if (hasCurrentCpf && !string.Equals(currentCpf, request.Cpf, StringComparison.Ordinal))
            result.Errors.Add(new ValidationFailure(nameof(request.Cpf), ResourceMessagesException.PERSON_CPF_UPDATE_NOT_ALLOWED));
        else if (!hasCurrentCpf && hasNewCpf)
        {
            var existCpf = await _personReadOnlyRepository.ExistPersonWithCpf(cpf: request.Cpf!, excludeId: personId);
            if (existCpf)
                result.Errors.Add(new ValidationFailure(nameof(request.Cpf), ResourceMessagesException.PERSON_CPF_ALREADY_REGISTERED));
        }

        if (!result.IsValid)
        {
            var errors = result.Errors.ToErrorsByPropertyDictionary();

            throw new ErrorOnValidationException(errors);
        }
    }
}
