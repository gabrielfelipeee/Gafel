using Gafel.Application.Exceptions;
using Gafel.Application.SharedResponses;
using Gafel.Application.Utils;
using Gafel.Domain.Identity.Dtos;
using Gafel.Domain.Identity.Interfaces;
using Gafel.Domain.Repositories.Person;

namespace Gafel.Application.UseCases.Auth.Login.DoLogin;

public class DoLoginUseCase : IDoLoginUseCase
{
    private readonly IAuthService _authService;
    private readonly IPersonReadOnlyRepository _personReadOnlyRepository;

    public DoLoginUseCase(IAuthService authService, IPersonReadOnlyRepository personReadOnlyRepository)
    {
        _authService = authService;
        _personReadOnlyRepository = personReadOnlyRepository;
    }

    public async Task<RegisteredUserResponse> Execute(DoLoginCommand request)
    {
        await Validate(request);

        var loginRequest = new LoginRequest()
        {
            Email = request.Email,
            Password = request.Password,
        };

        var result = await _authService.Login(loginRequest);
        if (!result.Success)
            throw new InvalidLoginException(result.ErrorMessage!);

        var person = await _personReadOnlyRepository.GetByUserId(result.UserId!.Value)
            ?? throw new UserPersonNotFoundException();

        return new RegisteredUserResponse
        {
            FullName = person.FullName,
            Tokens = new()
            {
                AccessToken = "AccessToken"
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
