namespace Gafel.Application.Utils;

public static class ValidationErrors
{
    public static Dictionary<string, string[]> ToDictionary(List<FluentValidation.Results.ValidationFailure> failures)
    {
        return failures
            .GroupBy(f => f.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(f => f.ErrorMessage).ToArray());
    }
}
