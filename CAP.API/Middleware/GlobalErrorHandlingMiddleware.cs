using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using CAP.API.Exceptions;
using Newtonsoft.Json;
using Sentry;

namespace CAP.API.Middleware;

/// <summary>
///  This class is used to handle exceptions that occur during the processing of a request.
///  It is added to the ASP.NET Core pipeline in the Startup class.
/// </summary>
public class GlobalErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    ///  This method is called by the ASP.NET Core runtime.
    /// </summary>
    /// <param name="context">
    /// The HttpContext for the current request.
    /// </param>
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        SentrySdk.CaptureException(exception);
        var exceptionType = exception.GetType();


        var exceptionResponse = exceptionType switch
        {
            _ when exceptionType == typeof(UnauthorizedAccessException) => new
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Message = "Unauthorized Access"
            },
            _ when exceptionType == typeof(NotFoundException) => new
            {
                StatusCode = HttpStatusCode.NotFound,
                Message = "Not Found"
            },
            _ when
                // All of these exceptions are bad requests
                new[]
                {
                    typeof(InvalidIdException), typeof(InvalidHashIdException), typeof(InvalidNetIdException),
                    typeof(InvalidPutIdRequestException), typeof(InvalidFileTypeException)
                }.Contains(exceptionType)
                => new
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Bad Request"
                },
            _ => new
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Message = "Internal Server Error"
            }
        };


        var serializedExceptionResponse = JsonConvert.SerializeObject(exceptionResponse);
        // Only run in dev
#if DEBUG
        // Dev response includes the stack trace
        var devResponse = new
        {
            exceptionResponse.StatusCode,
            exceptionResponse.Message,
            exception.StackTrace
        };
        serializedExceptionResponse = JsonConvert.SerializeObject(devResponse);
#endif


        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)exceptionResponse.StatusCode;

        return context.Response.WriteAsync(serializedExceptionResponse);
    }
}