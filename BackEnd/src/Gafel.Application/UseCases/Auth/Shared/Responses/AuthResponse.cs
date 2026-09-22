namespace Gafel.Application.UseCases.Auth.Shared.Responses;

public class AuthResponse
{
    public string FullName { get; set; } = string.Empty;
    public TokensResponse Tokens { get; set; } = default!;
}
