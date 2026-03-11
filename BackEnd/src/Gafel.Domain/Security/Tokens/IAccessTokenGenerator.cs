namespace Gafel.Domain.Security.Tokens;

public interface IAccessTokenGenerator
{
    public string Generate(long userId);
}
