using FluentValidation.Results;

namespace Gafel.Application.Extensions;

public static class ValidationFailureExtensions
{
    public static Dictionary<string, string[]> ToErrorsByPropertyDictionary(this List<ValidationFailure> failures)
    {
        return failures
            .GroupBy(f => f.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(f => f.ErrorMessage).ToArray());
    }
}
