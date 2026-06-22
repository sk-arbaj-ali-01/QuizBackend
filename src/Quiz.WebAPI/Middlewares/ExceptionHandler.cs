using Quiz.Shared.Exceptions;
using Quiz.Shared.Models;
using SqlException = MySqlConnector.MySqlException;

namespace Quiz.WebAPI.Middlewares;


public class ExceptionHandler(ILogger<ExceptionHandler> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (SqlException exception)
        {
            await HandleSqlException(context, exception);
        }
        catch (BaseException exception)
        {
            await HandleBaseException(context, exception);
        }
        catch (Exception exception)
        {
            await HandleGenericException(context, exception);
        }
    }

    private async Task HandleSqlException(HttpContext context, SqlException exception)
    {
        logger.LogError(exception, "Database error occurred: {Message}", exception.Message);

        ErrorDescriptionModel errorDescription = new()
        {
            StatusCode = (int)StatusCodes.Status500InternalServerError,
            Message = exception.Message,
            Description = "A database error occurred while processing the request."
        };

        context.Response.StatusCode = errorDescription.StatusCode;
        await context.Response.WriteAsJsonAsync(errorDescription);
    }

    private async Task HandleBaseException(HttpContext context, BaseException exception)
    {
        logger.LogError(exception, "Handled application exception: {Message}", exception.Message);

        ErrorDescriptionModel errorDescription = new()
        {
            StatusCode = exception.GetStatusCode(),
            Message = exception.Message ?? "An application error occurred.",
            Description = "The request could not be processed."
        };

        context.Response.StatusCode = errorDescription.StatusCode;
        await context.Response.WriteAsJsonAsync(errorDescription);
    }

    private async Task HandleGenericException(HttpContext context, Exception exception)
    {
        logger.LogError(exception, "Unhandled exception occurred: {Message}", exception.Message);

        ErrorDescriptionModel errorDescription = new()
        {
            StatusCode = (int)StatusCodes.Status500InternalServerError,
            Message = exception.Message,
            Description = "An unexpected error occurred while processing the request."
        };

        context.Response.StatusCode = errorDescription.StatusCode;
        await context.Response.WriteAsJsonAsync(errorDescription);
    }
}
