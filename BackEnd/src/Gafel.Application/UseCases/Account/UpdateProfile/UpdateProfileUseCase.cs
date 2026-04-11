using Gafel.Application.Exceptions;
using Gafel.Application.Extensions;
using Gafel.Domain.Entities;
using Gafel.Domain.Enums;
using Gafel.Domain.Repositories;
using Gafel.Domain.Repositories.Person;
using Gafel.Domain.Resources;
using Gafel.Domain.Services.CurrentUser;
using Gafel.Domain.ValueObjects;

namespace Gafel.Application.UseCases.Account.UpdateProfile;

public class UpdateProfileUseCase : IUpdateProfileUseCase
{
    private readonly ICurrentUser _currentUser;
    private readonly IPersonUpdateOnlyRepository _personUpdateOnlyRepository;
    private readonly IPersonReadOnlyRepository _personReadOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProfileUseCase(
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

    public async Task Execute(UpdateProfileCommand request)
    {
        Validate(request);

        var currentUser = _currentUser.CurrentUser();
        var person = await _personUpdateOnlyRepository.GetByUserId(currentUser.Id) ?? throw new PersonNotFoundException();

        var errors = new Dictionary<string, string[]>();

        // CPF
        await ProcessCpf(person: person, cpfRequest: request.Cpf, errors: errors);

        // Data de Nascimento
        ProcessDateOfBirth(person: person, dobRequest: request.DateOfBirth, errors: errors);

        if (errors.Count > 0)
            throw new ErrorOnValidationException(errors);

        // Atualização de campos mutáveis
        person.FullName = request.FullName;
        person.City = request.City;
        person.Uf = string.IsNullOrWhiteSpace(request.Uf) ? null : Enum.Parse<Uf>(request.Uf, true);
        person.UpdatedAt = DateTime.UtcNow;

        _personUpdateOnlyRepository.Update(person);
        await _unitOfWork.SaveChangesAsync();
    }

    private static void Validate(UpdateProfileCommand request)
    {
        var result = new UpdateProfileValidator().Validate(request);

        if (!result.IsValid)
        {
            var errors = result.Errors.ToErrorsByPropertyDictionary();

            throw new ErrorOnValidationException(errors);
        }
    }

    private async Task ProcessCpf(Person person, string? cpfRequest, Dictionary<string, string[]> errors)
    {
        if (string.IsNullOrWhiteSpace(cpfRequest))
            return;

        var cpfResult = Cpf.Create(cpfRequest);

        if (!cpfResult.IsSuccess)
        {
            AddError(errors, nameof(UpdateProfileCommand.Cpf), cpfResult.ErrorMessage!);
            return;
        }

        var newCpf = cpfResult.Value!;

        // Verificação de Unicidade
        if (person.Cpf is null && await _personReadOnlyRepository.ExistPersonWithCpf(cpf: newCpf, excludeId: person.Id))
        {
            AddError(errors, nameof(UpdateProfileCommand.Cpf), ResourceMessagesException.CPF_ALREADY_REGISTERED);
            return;
        }

        // Entidade decide se aceita (Imutabilidade)
        var updateResult = person.SetCpf(newCpf);
        if (!updateResult.IsSuccess)
            AddError(errors, nameof(UpdateProfileCommand.Cpf), updateResult.ErrorMessage!);
    }

    private static void ProcessDateOfBirth(Person person, DateOnly? dobRequest, Dictionary<string, string[]> errors)
    {
        if (!dobRequest.HasValue)
            return;

        var dobResult = DateOfBirth.Create(dobRequest.Value);

        if (!dobResult.IsSuccess)
        {
            AddError(errors, nameof(UpdateProfileCommand.DateOfBirth), dobResult.ErrorMessage!);
            return;
        }

        // Entidade decide se aceita (Imutabilidade)
        var updateResult = person.SetDateOfBirth(dobResult.Value!);

        if (!updateResult.IsSuccess)
            AddError(errors, nameof(UpdateProfileCommand.DateOfBirth), updateResult.ErrorMessage!);
    }

    private static void AddError(Dictionary<string, string[]> errors, string key, string message)
    {
        if (errors.TryGetValue(key, out string[]? value))
            errors[key] = [.. value, message];
        else
            errors.Add(key, [message]);
    }
}
