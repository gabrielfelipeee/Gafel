using System.Net;
using Gafel.Domain.Resources;
namespace Gafel.Application.Exceptions;

public class ErrorOnValidationException(Dictionary<string, string[]> errors) : GafelException(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_DETAIL)
{
    private readonly Dictionary<string, string[]> _errors = errors;

    public Dictionary<string, string[]> GetErrors() => _errors;

    public override string GetErrorTitle() => ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_TITLE;

    public override string GetErrorDetail() => Message;

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.BadRequest;
}