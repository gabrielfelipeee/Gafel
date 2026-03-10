namespace Gafel.Application.UseCases.Auth.Login.DoLogin;

public class DoLoginCommand
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
