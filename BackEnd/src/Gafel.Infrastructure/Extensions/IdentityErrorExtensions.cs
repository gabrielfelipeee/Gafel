using Microsoft.AspNetCore.Identity;

namespace Gafel.Infrastructure.Extensions;

public static class IdentityErrorExtensions
{
    public static Dictionary<string, string[]> ToErrorsByCodeDictionary(this IEnumerable<IdentityError> errors)
    {
        return errors
            .GroupBy(e => e.Code)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.Description).Distinct().ToArray());
    }
}
