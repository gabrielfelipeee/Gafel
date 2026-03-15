using Gafel.Application.Exceptions;
using Gafel.Application.UseCases.Auth.SharedResponses;
using Gafel.Application.Utils;
using Gafel.Domain.Identity.Dtos;
using Gafel.Domain.Identity.Interfaces;
using Gafel.Domain.Repositories;
using Gafel.Domain.Repositories.Person;
using Gafel.Domain.Security.Tokens;
using Mapster;

namespace Gafel.Application.UseCases.Auth.Register;

public class RegisterAccountUseCase : IRegisterAccountUseCase
{
    private readonly IPersonWriteOnlyRepository _personWriteOnlyRepository;
    private readonly IUserWriteOnlyService _identityWriteOnly;
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterAccountUseCase(
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

    public async Task<AuthResponse> Execute(RegisterAccountCommand request)
    {
        await Validate(request);

        AuthResponse response = null!;

        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var credentials = request.Adapt<UserCredentialsDto>();

            var result = await _identityWriteOnly.Register(credentials);
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

            response = new AuthResponse
            {
                FullName = person.FullName,
                Tokens = new()
                {
                    AccessToken = _accessTokenGenerator.Generate(userId)
                }
            };
        });

        return response;
    }

    private static async Task Validate(RegisterAccountCommand request)
    {
        var result = new RegisterAccountValidator().Validate(request);

        if (!result.IsValid)
        {
            var errors = ValidationErrors.ToDictionary(result.Errors);

            throw new ErrorOnValidationException(errors);
        }
    }
}
