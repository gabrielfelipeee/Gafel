using FluentValidation;
using Gafel.Domain.Resources;
using System.Text.RegularExpressions;

namespace Gafel.Application.RuleExtensions;

public static partial class PasswordRuleExtension
{
    public static IRuleBuilderOptions<T, string> PasswordPolicy<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .MinimumLength(8)
            .WithMessage(ResourceMessagesException.USER_PASSWORD_TOO_SHORT)

            .Matches(ContainsDigit())
            .WithMessage(ResourceMessagesException.USER_PASSWORD_REQUIRES_NUMBER)

            .Matches(ContainsSpecialCharacter())
            .WithMessage(ResourceMessagesException.USER_PASSWORD_REQUIRES_SPECIAL_CHAR);
    }

    [GeneratedRegex(@"\d")]
    private static partial Regex ContainsDigit();

    [GeneratedRegex(@"[\W_]")]
    private static partial Regex ContainsSpecialCharacter();
}
