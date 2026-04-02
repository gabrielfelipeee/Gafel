using Gafel.Application.Exceptions;
using Gafel.Domain.Constants;
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
            context.Response.ContentType = "application/problem+json";

            var problem = new ProblemDetails
            {
                Title = ResourceMessagesException.UNAUTHORIZED,
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
        // Header ausente
        if (!context.Request.Headers.ContainsKey("Authorization"))
            return ResourceMessagesException.AUTH_NO_TOKEN;

        // Tenta obter a exceção armazenada no pipeline de autenticação 
        if (!context.Items.TryGetValue(HttpContextKeys.AuthException, out var exception))
            return ResourceMessagesException.AUTH_TOKEN_INVALID;

        return exception switch
        {
            SecurityTokenExpiredException => ResourceMessagesException.AUTH_TOKEN_EXPIRED,
            InvalidUserTokenException ex => ex.GetErrorDetail(),
            _ => ResourceMessagesException.AUTH_TOKEN_INVALID
        };
    }
}
