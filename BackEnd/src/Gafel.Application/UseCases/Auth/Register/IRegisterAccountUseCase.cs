using Gafel.Application.SharedResponses;

namespace Gafel.Application.UseCases.Auth.Register;

public interface IRegisterAccountUseCase
{
    Task<RegisteredUserResponse> Execute(RegisterAccountCommand request);
}
