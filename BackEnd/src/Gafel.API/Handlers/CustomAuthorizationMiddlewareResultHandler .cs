using Gafel.Domain.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Gafel.API.Handlers;

/// <summary>
/// Customizador de respostas para falhas de autorização.
/// Intercepta o resultado do AuthorizationMiddleware antes da resposta ser enviada.
/// </summary>
public class CustomAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
{
    private readonly AuthorizationMiddlewareResultHandler _defaultHandler = new();

    public async Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
    {
        // Se falhou na autenticação (token inválido, expirado ou inexistente)
        if (authorizeResult.Challenged)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;

            var problem = new ProblemDetails
            {
                Title = "Não autorizado",
                Detail = GetDetailMessage(context),
                Status = StatusCodes.Status401Unauthorized,
                Instance = context.Request.Path
            };

            await context.Response.WriteAsJsonAsync(problem);
            return;
        }

        await _defaultHandler.HandleAsync(next, context, policy, authorizeResult);
    }

    private static string GetDetailMessage(HttpContext context)
    {
        var authHeader = context.Request.Headers.Authorization.ToString();

        // Header ausente
        if (string.IsNullOrWhiteSpace(authHeader))
            return ResourceMessagesException.NO_AUTH_TOKEN;

        // Token expirado
        if (context.Items["JwtException"] is SecurityTokenExpiredException)
            return ResourceMessagesException.AUTH_TOKEN_EXPIRED;

        // Token inválido
        return ResourceMessagesException.AUTH_TOKEN_INVALID;
    }
}
