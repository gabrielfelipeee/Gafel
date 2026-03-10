using System.Net;

namespace Gafel.Application.Exceptions;

public abstract class GafelException(string message) : SystemException(message)
{
    public abstract string GetErrorTitle();
    public abstract string GetErrorDetail();
    public abstract HttpStatusCode GetStatusCode();
}
