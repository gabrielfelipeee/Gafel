using Gafel.Application.UseCases.Auth.SharedResponses;

namespace Gafel.Application.UseCases.Auth.Login.DoLogin;

public interface IDoLoginUseCase
{
    Task<AuthResponse> Execute(DoLoginCommand request);
}
