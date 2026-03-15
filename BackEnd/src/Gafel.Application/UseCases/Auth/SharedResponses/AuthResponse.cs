namespace Gafel.Application.UseCases.Auth.SharedResponses;

public class AuthResponse
{
    public string FullName { get; set; } = string.Empty;
    public TokensResponse Tokens { get; set; } = default!;
}
