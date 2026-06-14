using System.Net;

namespace Quiz.Shared.Exceptions.DatabaseExceptions;
public class DatabaseException(
    string? message,
    HttpStatusCode statusCode) 
    : BaseException(
        message,
        statusCode)
{
}
