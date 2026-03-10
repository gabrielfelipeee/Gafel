using Gafel.Application.SharedResponses;

namespace Gafel.Application.UseCases.Auth.Login.DoLogin;

public interface IDoLoginUseCase
{
    Task<RegisteredUserResponse> Execute(DoLoginCommand request);
}
