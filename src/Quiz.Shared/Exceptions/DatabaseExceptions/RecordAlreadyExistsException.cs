using System.Net;

namespace Quiz.Shared.Exceptions.DatabaseExceptions;
public class RecordAlreadyExistsException(
    string message = "Record already exists",
    HttpStatusCode statusCode = HttpStatusCode.Conflict)
    : BaseException(message, statusCode)
{
}
