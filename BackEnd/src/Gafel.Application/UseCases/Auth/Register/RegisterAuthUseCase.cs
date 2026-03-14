using Gafel.Application.Exceptions;
using Gafel.Application.SharedResponses;
using Gafel.Application.Utils;
using Gafel.Domain.Identity.Dtos;
using Gafel.Domain.Identity.Interfaces;
using Gafel.Domain.Repositories;
using Gafel.Domain.Repositories.Person;
using Gafel.Domain.Security.Tokens;
using Mapster;

namespace Gafel.Application.UseCases.Auth.Register;

public class RegisterAuthUseCase : IRegisterAuthUseCase
{
    private readonly IPersonWriteOnlyRepository _personWriteOnlyRepository;
    private readonly IUserWriteOnlyService _identityWriteOnly;
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterAuthUseCase(
        IPersonWriteOnlyRepository personWriteOnlyRepository,
        IUserWriteOnlyService identityUserWriteOnlyService,
        IAccessTokenGenerator accessTokenGenerator,
        IUnitOfWork unitOfWork
        )
    {
        _personWriteOnlyRepository = personWriteOnlyRepository;
        _identityWriteOnly = identityUserWriteOnlyService;
        _accessTokenGenerator = accessTokenGenerator;
        _unitOfWork = unitOfWork;
    }

    public async Task<RegisteredUserResponse> Execute(RegisterAuthCommand request)
    {
        await Validate(request);

        await using var transaction = await _unitOfWork.BeginTransactionAsync();
        try
        {
            var userRequest = request.Adapt<RegisterUserRequest>();

            var result = await _identityWriteOnly.Register(userRequest);
            if (!result.Success)
                throw new ErrorOnValidationException(result.Errors!);

            var userId = result.UserId!.Value;

            var person = new Domain.Entities.Person()
            {
                FullName = request.FullName,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
            };
            await _personWriteOnlyRepository.Add(person);

            await _unitOfWork.SaveChangesAsync();
            await transaction.CommitAsync();

            return new RegisteredUserResponse
            {
                FullName = person.FullName,
                Tokens = new()
                {
                    AccessToken = _accessTokenGenerator.Generate(userId)
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
