using Gafel.Application.UseCases.Auth.Shared.Responses;

namespace Gafel.Application.UseCases.Auth.Register;

public interface IRegisterAccountUseCase
{
    Task<AuthResponse> Execute(RegisterAccountCommand request);
}
