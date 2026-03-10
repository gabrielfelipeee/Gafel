using Gafel.Application.Exceptions;
using Gafel.Application.SharedResponses;
using Gafel.Application.Utils;
using Gafel.Domain.Identity.Dtos;
using Gafel.Domain.Identity.Interfaces;
using Gafel.Domain.Repositories;
using Gafel.Domain.Repositories.Person;

namespace Gafel.Application.UseCases.Auth.Register;

public class RegisterAuthUseCase : IRegisterAuthUseCase
{
    private readonly IPersonWriteOnlyRepository _personWriteOnlyRepository;
    private readonly IUserWriteOnlyService _identityWriteOnly;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterAuthUseCase(
        IPersonWriteOnlyRepository personWriteOnlyRepository,
        IUserWriteOnlyService identityUserWriteOnlyService,
        IUnitOfWork unitOfWork
        )
    {
        _personWriteOnlyRepository = personWriteOnlyRepository;
        _identityWriteOnly = identityUserWriteOnlyService;
        _unitOfWork = unitOfWork;
    }

    public async Task<RegisteredUserResponse> Execute(RegisterAuthCommand request)
    {
        await Validate(request);

        var transaction = await _unitOfWork.BeginTransactionAsync();
        try
        {
            var result = await _identityWriteOnly.Register(new RegisterUserRequest() { Email = request.Email, Password = request.Password });
            if (!result.Success)
                throw new ErrorOnValidationException(result.Errors!);

            var person = new Domain.Entities.Person()
            {
                FullName = request.FullName,
                UserId = result.UserId!.Value,
                CreatedAt = DateTime.UtcNow,
            };
            await _personWriteOnlyRepository.Add(person);

            await _unitOfWork.SaveChangesAsync();

            // Aqui gera o token -> Ainda será implementado

            await transaction.CommitAsync();

            return new RegisteredUserResponse
            {
                FullName = person.FullName,
                Tokens = new()
                {
                     AccessToken = "AccessToken"
                }
            };
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    private static async Task Validate(RegisterAuthCommand request)
    {
        var result = new RegisterAuthValidator().Validate(request);

        if (!result.IsValid)
        {
            var errors = ValidationErrors.ToDictionary(result.Errors);

            throw new ErrorOnValidationException(errors);
        }
    }
}
