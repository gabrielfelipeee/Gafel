using System.Text.RegularExpressions;
using DocumentValidator;
using Gafel.Domain.Resources;
using Gafel.Domain.Common;

namespace Gafel.Domain.ValueObjects;

public sealed partial record Cpf
{
    public string Value { get; }
    private Cpf(string value) => Value = value;

    public static Result<Cpf> Create(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return Result<Cpf>.Failure(errorMessage: ResourceMessagesException.CPF_EMPTY);

        var normalized = RemoveNonDigits(input);

        if (!IsValid(normalized))
            return Result<Cpf>.Failure(errorMessage: ResourceMessagesException.CPF_INVALID);

        return Result<Cpf>.Success(value: new Cpf(normalized));
    }

    private static string RemoveNonDigits(string input) => OnlyNumbers().Replace(input, "");

    private static bool IsValid(string cpf) => CpfValidation.Validate(cpf);


    [GeneratedRegex("[^0-9]")]
    private static partial Regex OnlyNumbers();
}
