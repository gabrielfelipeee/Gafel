namespace Gafel.Application.UseCases.Person.Update;

public class UpdatePersonCommand
{
    public string FullName { get; set; } = string.Empty;
    public string? Cpf { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public string? Uf { get; set; }
    public string? City { get; set; }
}
