using Gafel.Domain.Resources;
using System.Net;

namespace Gafel.Application.Exceptions;

public class PersonNotFoundException() : GafelException(ResourceMessagesException.EXCEPTION_PERSON_NOT_FOUND_DETAIL)
{
    public override string GetErrorTitle() => ResourceMessagesException.EXCEPTION_PERSON_NOT_FOUND_TITLE;
    public override string GetErrorDetail() => Message;
    public override HttpStatusCode GetStatusCode() => HttpStatusCode.NotFound;
}
