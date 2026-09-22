using Gafel.Application.UseCases.Auth.Shared.Responses;

namespace Gafel.Application.UseCases.Auth.Login.DoLogin;

public interface IDoLoginUseCase
{
    Task<AuthResponse> Execute(DoLoginCommand request);
}
