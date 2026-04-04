namespace Gafel.Application.UseCases.Account.GetProfile;

public interface IGetProfileUseCase
{
    Task<GetProfileResponse> Execute();
}
