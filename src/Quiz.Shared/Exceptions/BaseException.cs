using System.Net;

namespace Quiz.Shared.Exceptions;
public class BaseException(
    string? message,
    HttpStatusCode statusCode = HttpStatusCode.InternalServerError
    ) : Exception(message)
{
    public int GetStatusCode()
    {
        return ((int)statusCode);
    }
}
