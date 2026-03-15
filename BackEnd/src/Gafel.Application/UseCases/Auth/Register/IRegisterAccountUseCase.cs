using Gafel.Application.UseCases.Auth.SharedResponses;

namespace Gafel.Application.UseCases.Auth.Register;

public interface IRegisterAccountUseCase
{
    Task<AuthResponse> Execute(RegisterAccountCommand request);
}
