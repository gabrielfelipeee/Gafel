using Gafel.Application.Exceptions;
using Gafel.Domain.Resources;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Gafel.API.Handlers;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var problem = CreateProblemDetails(exception);

        problem.Instance = httpContext.Request.Path;

        httpContext.Response.StatusCode = problem.Status ?? StatusCodes.Status500InternalServerError;


        // (object) Força a serialização do tipo real (ex: ValidationProblemDetails) 
        // em vez do tipo base (ProblemDetails), garantindo que a propriedade 'Errors' seja incluída.
        await httpContext.Response.WriteAsJsonAsync((object)problem, cancellationToken);

        return true; // Indica que a exceção foi tratada
    }

    private static ProblemDetails CreateProblemDetails(Exception exception) => exception switch
    {
        ErrorOnValidationException validationException => new ValidationProblemDetails(validationException.GetErrors())
        {
            Title = validationException.GetErrorTitle(),
            Detail = validationException.GetErrorDetail(),
            Status = (int)validationException.GetStatusCode()
        },

        GafelException gafelException => new ProblemDetails
        {
            Title = gafelException.GetErrorTitle(),
            Detail = gafelException.GetErrorDetail(),
            Status = (int)gafelException.GetStatusCode()
        },

        _ => new ProblemDetails
        {
            Title = "Erro Interno do Servidor",
            Detail = ResourceMessagesException.UNKNOWN_ERROR,
            Status = StatusCodes.Status500InternalServerError
        }
    };
}
