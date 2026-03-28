using Gafel.Domain.Resources;
using System.Net;

namespace Gafel.Application.Exceptions;

public class InvalidUserTokenException() : GafelException(ResourceMessagesException.EXCEPTION_INVALID_USER_TOKEN_DETAIL)
{
    public override string GetErrorTitle() => ResourceMessagesException.UNAUTHORIZED;
    public override string GetErrorDetail() => Message;
    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}
