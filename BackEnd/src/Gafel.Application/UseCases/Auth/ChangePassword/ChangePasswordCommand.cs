namespace Gafel.Application.UseCases.Auth.ChangePassword;

public class ChangePasswordCommand
{
    public string Password { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}
