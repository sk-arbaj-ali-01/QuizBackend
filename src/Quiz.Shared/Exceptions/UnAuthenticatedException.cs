using System.Net;

namespace Quiz.Shared.Exceptions;
public class UnAuthenticatedException(
    string message = "Invalid credentials",
    HttpStatusCode statusCode = HttpStatusCode.Unauthorized)
    : BaseException(message, statusCode)
{
}
