namespace Gafel.Application.UseCases.Account.GetProfile;

public class GetProfileResponse
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Cpf { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public string? Uf { get; set; }
    public string? City { get; set; }
}
