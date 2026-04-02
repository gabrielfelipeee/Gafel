using Gafel.Domain.Common;
using Gafel.Domain.Enums;
using Gafel.Domain.Resources;
using Gafel.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gafel.Domain.Entities;

[Table("people")]
public class Person : EntityBase
{
    public string FullName { get; set; } = string.Empty;
    public Cpf? Cpf { get; private set; }
    public DateOfBirth? DateOfBirth { get; private set; }
    public Uf? Uf { get; set; }
    public string? City { get; set; }

    public long UserId { get; set; }

    public Result<bool> SetCpf(Cpf cpf)
    {
        // já tem CPF -> não pode mudar
        if (Cpf is not null)
        {
            if (!Cpf.Equals(cpf))
                return Result<bool>.Failure(ResourceMessagesException.CPF_UPDATE_NOT_ALLOWED);

            return Result<bool>.Success(true); // mesmo CPF
        }

        Cpf = cpf;
        return Result<bool>.Success(value: true);
    }

    public Result<bool> SetDateOfBirth(DateOfBirth dateOfBirth)
    {
        // já tem data de nascimento -> não pode mudar
        if (DateOfBirth is not null)
        {
            if (!DateOfBirth.Equals(dateOfBirth))
                return Result<bool>.Failure(ResourceMessagesException.PERSON_DATE_OF_BIRTH_UPDATE_NOT_ALLOWED);

            return Result<bool>.Success(true); // mesma data de nascimento
        }

        DateOfBirth = dateOfBirth;
        return Result<bool>.Success(value: true);
    }
}
