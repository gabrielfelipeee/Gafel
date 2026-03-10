using Gafel.Domain.Resources;
using System.Net;

namespace Gafel.Application.Exceptions;

public class UserPersonNotFoundException() : GafelException(ResourceMessagesException.PERSON_NOT_FOUND)
{
    public override string GetErrorTitle() => "Perfil não encontrado";
    public override string GetErrorDetail() => Message;
    public override HttpStatusCode GetStatusCode() => HttpStatusCode.NotFound;
}
