using System.Net;

namespace Quiz.Shared.Exceptions.DatabaseExceptions;
public class RecordNotFoundException(
    string? message,
    HttpStatusCode statusCode = HttpStatusCode.NotFound) 
    : DatabaseException(
        message,
        statusCode)
{
}
