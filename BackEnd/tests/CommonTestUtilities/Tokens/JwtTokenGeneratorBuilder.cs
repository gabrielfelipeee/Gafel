using Gafel.Domain.Security.Tokens;
using Gafel.Infrastructure.Security.Tokens.Access;

namespace CommonTestUtilities.Tokens;

public static class JwtTokenGeneratorBuilder
{
    public static IAccessTokenGenerator Build() => new JwtTokenGenerator(expirationTimeMinutes: 5, signinKey: "z6H5w2P9dM8VnQfLxJ1TgKz7YsEw3CpL");
}
