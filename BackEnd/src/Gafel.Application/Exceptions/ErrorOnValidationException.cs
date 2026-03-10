using System.Net;

namespace Gafel.Application.Exceptions;

public class ErrorOnValidationException(Dictionary<string, string[]> errors) : GafelException("Um ou mais erros de validação ocorreram.")
{
    private readonly Dictionary<string, string[]> _errors = errors;

    public Dictionary<string, string[]> GetErrors() => _errors;

    public override string GetErrorTitle() => "Erro de validação";

    public override string GetErrorDetail() => Message;

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.BadRequest;
}