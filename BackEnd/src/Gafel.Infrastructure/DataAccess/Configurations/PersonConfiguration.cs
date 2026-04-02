using Gafel.Domain.Entities;
using Gafel.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gafel.Infrastructure.DataAccess.Configurations;

public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        // Configura a propriedade Uf para ser armazenada como string no banco de dados,
        builder.Property(person => person.Uf)
            .HasConversion<string>()
            .HasMaxLength(2);

        builder
            .Property(person => person.Cpf)
            .HasConversion(
                cpf => cpf == null ? null : cpf.Value, // Para o banco (salvando)
                value => value == null ? null : Cpf.Create(value).Value // Do banco (lendo)
            ); 

        builder
            .Property(person => person.DateOfBirth)
            .HasConversion(
                dateOfBirth => dateOfBirth == null ? (DateTime?)null : dateOfBirth.Value.ToDateTime(TimeOnly.MinValue),
                value => value == null ? null : DateOfBirth.Create(DateOnly.FromDateTime(value.Value)).Value
            );
    }
}
