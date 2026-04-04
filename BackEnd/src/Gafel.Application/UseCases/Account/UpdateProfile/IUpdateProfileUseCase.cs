namespace Gafel.Application.UseCases.Account.UpdateProfile;

public interface IUpdateProfileUseCase
{
    Task Execute(UpdateProfileCommand request);
}
