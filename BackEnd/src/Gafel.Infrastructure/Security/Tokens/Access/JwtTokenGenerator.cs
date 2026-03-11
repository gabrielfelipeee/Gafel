using Gafel.Domain.Security.Tokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Gafel.Infrastructure.Security.Tokens.Access;

public class JwtTokenGenerator(uint expirationTimeMinutes, string signinKey) : JwtTokenHandler, IAccessTokenGenerator
{
    private readonly uint _expirationTimeMinutes = expirationTimeMinutes;
    private readonly string _signinKey = signinKey;

    public string Generate(long userId)
    {
        // Lista de claims (informações) que estarão dentro do token
        var claims = new List<Claim>()
        {
            new(ClaimTypes.Sid, userId.ToString())
        };

        // Descrição do token a ser criado
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            // Define quando o token vai expirar
            Expires = DateTime.UtcNow.AddMinutes(_expirationTimeMinutes),

            // Define a credencial de assinatura com a chave secreta e o algoritmo HMAC-SHA256
            SigningCredentials = new SigningCredentials(SecurityKey(_signinKey), SecurityAlgorithms.HmacSha256Signature),

            // Define quem é o "dono" do token (as claims)
            Subject = new ClaimsIdentity(claims)
        };

        // Criador e manipulador de tokens JWT
        var tokenHandler = new JwtSecurityTokenHandler();

        // Cria o token baseado na descrição
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);

        // Converte o token para string no formato JWT
        return tokenHandler.WriteToken(securityToken);
    }
}
