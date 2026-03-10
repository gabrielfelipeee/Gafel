using System.ComponentModel.DataAnnotations.Schema;

namespace Gafel.Domain.Entities;

[Table("people")]
public class Person : EntityBase
{
    public string FullName { get; set; } = string.Empty;
    public string? Cpf { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public string? City { get; set; }
    public string? Uf { get; set; }

    public long UserId { get; set; }
}
