namespace Gafel.Application.UseCases.Auth.ChangePassword;

public interface IChangePasswordUseCase
{
    Task Execute(ChangePasswordCommand request);
}
