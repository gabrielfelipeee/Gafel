using FluentValidation;
using Gafel.Domain.Resources;
using System.Text.RegularExpressions;

namespace Gafel.Application.RuleExtensions;

public static partial class PasswordRuleExtension
{
    public static IRuleBuilderOptions<T, string> PasswordRequirements<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
                .WithMessage(ResourceMessagesException.PASSWORD_EMPTY)

            .MinimumLength(8)
                .WithMessage(ResourceMessagesException.PASSWORD_TOO_SHORT)

            .Matches(ContainsDigit())
                .WithMessage(ResourceMessagesException.PASSWORD_REQUIRES_NUMBER)

            .Matches(ContainsSpecialCharacter())
                .WithMessage(ResourceMessagesException.PASSWORD_REQUIRES_SPECIAL_CHARACTER);
    }

    [GeneratedRegex(@"\d")]
    private static partial Regex ContainsDigit();

    [GeneratedRegex(@"[\W_]")]
    private static partial Regex ContainsSpecialCharacter();
}
