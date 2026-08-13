using System.Net;
using System.Text.Json;

namespace PWA_API.Api.Middleware;

public class GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            // The trace id goes into the log line *and* into the response body,
            // so a user-reported error code can be located in the log file and
            // in the audit trail without guessing.
            logger.LogError(ex,
                "Excepción no controlada en {Method} {Path} (trace {TraceId}): {Message}",
                context.Request.Method, context.Request.Path.Value, context.GetTraceId(), ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, message) = exception switch
        {
            UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "Unauthorized access."),
            KeyNotFoundException => (HttpStatusCode.NotFound, exception.Message),
            ArgumentException => (HttpStatusCode.BadRequest, exception.Message),
            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred.")
        };

        context.Response.StatusCode = (int)statusCode;

        var response = JsonSerializer.Serialize(new
        {
            status = (int)statusCode,
            error = message,
            traceId = context.GetTraceId()
        });

        return context.Response.WriteAsync(response);
    }
}
