using Gafel.Application.Exceptions;
using Gafel.Application.SharedResponses;
using Gafel.Application.Utils;
using Gafel.Domain.Identity.Dtos;
using Gafel.Domain.Identity.Interfaces;
using Gafel.Domain.Repositories.Person;
using Gafel.Domain.Security.Tokens;
using Mapster;

namespace Gafel.Application.UseCases.Auth.Login.DoLogin;

public class DoLoginUseCase : IDoLoginUseCase
{
    private readonly IAuthService _authService;
    private readonly IPersonReadOnlyRepository _personReadOnlyRepository;
    private readonly IAccessTokenGenerator _accessTokenGenerator;

    public DoLoginUseCase(IAuthService authService, IPersonReadOnlyRepository personReadOnlyRepository, IAccessTokenGenerator accessTokenGenerator)
    {
        _authService = authService;
        _personReadOnlyRepository = personReadOnlyRepository;
        _accessTokenGenerator = accessTokenGenerator;
    }

    public async Task<RegisteredUserResponse> Execute(DoLoginCommand request)
    {
        await Validate(request);

        var credentials = request.Adapt<UserCredentialsDto>();

        var result = await _authService.Login(credentials);
        if (!result.Success)
            throw new InvalidLoginException(result.ErrorMessage!);

        var userId = result.UserId!.Value;

        var person = await _personReadOnlyRepository.GetByUserId(userId)
            ?? throw new UserPersonNotFoundException();

        return new RegisteredUserResponse
        {
            FullName = person.FullName,
            Tokens = new()
            {
                AccessToken = _accessTokenGenerator.Generate(userId)
            }
        };
    }

    private static async Task Validate(DoLoginCommand request)
    {
        var result = new DoLoginValidator().Validate(request);

        if (!result.IsValid)
        {
            var errors = ValidationErrors.ToDictionary(result.Errors);
            throw new ErrorOnValidationException(errors);
        }
    }
}
