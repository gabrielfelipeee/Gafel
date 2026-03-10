using System.Net;

namespace Gafel.Application.Exceptions;

public class InvalidLoginException(string errorMessage) : GafelException(errorMessage)
{

    public override string GetErrorTitle() => "Falha na autenticação";
    public override string GetErrorDetail() => Message;
    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}
