using Gafel.Application.SharedResponses;

namespace Gafel.Application.UseCases.Auth.Register;

public interface IRegisterAuthUseCase
{
    Task<RegisteredUserResponse> Execute(RegisterAuthCommand request);
}
