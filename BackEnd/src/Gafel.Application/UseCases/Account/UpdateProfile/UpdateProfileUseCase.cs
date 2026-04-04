using Gafel.Application.Exceptions;
using Gafel.Application.Extensions;
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
        if (!string.IsNullOrWhiteSpace(request.Cpf))
        {
            var cpfResult = Cpf.Create(request.Cpf);

            if (!cpfResult.IsSuccess)
                AddError(errors, nameof(request.Cpf), cpfResult.ErrorMessage!);
            else
            {
                var newCpf = cpfResult.Value!;

                // Só consulta o banco se a pessoa estiver tentando cadastrar um CPF e ainda não tiver um. (Unicidade)
                if (person.Cpf is null && await _personReadOnlyRepository.ExistPersonWithCpf(cpf: newCpf, excludeId: person.Id))
                    AddError(errors, nameof(request.Cpf), ResourceMessagesException.CPF_ALREADY_REGISTERED);
                else
                {
                    // Entidade decide se aceita (Imutabilidade)
                    var result = person.SetCpf(newCpf);
                    if (!result.IsSuccess)
                        AddError(errors, nameof(request.Cpf), result.ErrorMessage!);
                }
            }
        }

        // Data de Nascimento
        if (request.DateOfBirth.HasValue)
        {
            var dobResult = DateOfBirth.Create(request.DateOfBirth.Value);

            if (!dobResult.IsSuccess)
                AddError(errors, nameof(request.DateOfBirth), dobResult.ErrorMessage!);
            else
            {
                // Entidade decide se aceita (Imutabilidade)
                var result = person.SetDateOfBirth(dobResult.Value!);
                if (!result.IsSuccess)
                    AddError(errors, nameof(request.DateOfBirth), result.ErrorMessage!);
            }
        }

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

    private static void AddError(Dictionary<string, string[]> errors, string key, string message)
    {
        if (errors.TryGetValue(key, out string[]? value))
            errors[key] = [.. value, message];
        else
            errors.Add(key, [message]);
    }
}
