using Gafel.Domain.Resources;
using System.Net;

namespace Gafel.Application.Exceptions;

public class InvalidLoginException(string errorMessage) : GafelException(errorMessage)
{
    public override string GetErrorTitle() => ResourceMessagesException.EXCEPTION_INVALID_LOGIN_TITLE;
    public override string GetErrorDetail() => Message;
    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}
