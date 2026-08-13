using Serilog.Context;

namespace PWA_API.Api.Middleware;

/// <summary>
/// Assigns a trace id to every request and makes it visible in three places at
/// once: the <c>X-Trace-Id</c> response header (so the client can show it to the
/// user), the Serilog context (so every log line of that request carries it) and
/// the audit record written by <see cref="AuditMiddleware"/>. Those three share
/// the same value, which is what makes a reported failure traceable end to end.
/// </summary>
public class RequestTraceMiddleware(RequestDelegate next)
{
    public const string HeaderName = "X-Trace-Id";
    public const string ItemKey = "TraceId";

    public async Task InvokeAsync(HttpContext context)
    {
        // Honour a trace id supplied by the caller (useful when the frontend
        // already generated one), otherwise derive it from the request id.
        var traceId = context.Request.Headers[HeaderName].FirstOrDefault();
        if (string.IsNullOrWhiteSpace(traceId))
            traceId = context.TraceIdentifier.Replace(":", "-");

        context.Items[ItemKey] = traceId;
        context.Response.Headers[HeaderName] = traceId;

        using (LogContext.PushProperty("TraceId", traceId))
        {
            await next(context);
        }
    }
}

public static class HttpContextTraceExtensions
{
    public static string GetTraceId(this HttpContext context) =>
        context.Items[RequestTraceMiddleware.ItemKey] as string ?? context.TraceIdentifier;
}
