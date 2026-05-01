using Gafel.Domain.Resources;
using System.Net;

namespace Gafel.Application.Exceptions;

public class NotFoundException(string errorMessage) : GafelException(errorMessage)
{
    public override string GetErrorTitle() => ResourceMessagesException.EXCEPTION_NOT_FOUND_TITLE;
    public override string GetErrorDetail() => Message;
    public override HttpStatusCode GetStatusCode() => HttpStatusCode.NotFound;
}
