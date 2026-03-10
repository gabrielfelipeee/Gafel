namespace Gafel.Application.SharedResponses;
public class RegisteredUserResponse
{
    public string FullName { get; set; } = string.Empty;
    public TokensResponse Tokens { get; set; } = default!;
}
