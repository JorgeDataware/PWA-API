using System.Diagnostics;
using System.Security.Claims;
using PWA_API.Api.Extensions;
using PWA_API.Application.Interfaces.Services;
using PWA_API.Application.Services;
using PWA_API.Domain.Entities;

namespace PWA_API.Api.Middleware;

/// <summary>
/// Writes one audit record per state-changing request (anything that is not a
/// GET), whether it succeeded or failed. Auditing here instead of inside each
/// endpoint means no operation can be added later and silently escape the trail.
/// </summary>
public class AuditMiddleware(RequestDelegate next, ILogger<AuditMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (HttpMethods.IsGet(context.Request.Method) ||
            HttpMethods.IsOptions(context.Request.Method) ||
            HttpMethods.IsHead(context.Request.Method))
        {
            await next(context);
            return;
        }

        var stopwatch = Stopwatch.StartNew();
        try
        {
            await next(context);
        }
        finally
        {
            stopwatch.Stop();
            // Auditing must never break the request it is auditing: a failure
            // to persist the record is logged and swallowed.
            try
            {
                await WriteEntryAsync(context, stopwatch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "No se pudo escribir el registro de auditoría de {Method} {Path}",
                    context.Request.Method, context.Request.Path.Value);
            }
        }
    }

    private static async Task WriteEntryAsync(HttpContext context, long elapsedMs)
    {
        var auditService = context.RequestServices.GetRequiredService<IAuditService>();
        var method = context.Request.Method;
        var path = context.Request.Path.Value ?? string.Empty;
        var status = context.Response.StatusCode;
        var userId = context.User.Identity?.IsAuthenticated == true ? context.User.GetUserId() : (int?)null;

        var entry = new AuditLog
        {
            OccurredAt = DateTime.UtcNow,
            TraceId = context.GetTraceId(),
            UserId = userId == 0 ? null : userId,
            Username = context.User.FindFirstValue(ClaimTypes.Name)
                       ?? context.User.FindFirstValue("username"),
            Role = context.User.FindFirstValue(ClaimTypes.Role),
            Method = method,
            Path = Truncate(path, 300),
            Action = AuditService.DescribeAction(method, path),
            StatusCode = status,
            Success = status is >= 200 and < 400,
            DurationMs = (int)elapsedMs,
            IpAddress = context.Connection.RemoteIpAddress?.ToString(),
            Error = status >= 400 ? Truncate(ReasonFor(status), 500) : null
        };

        await auditService.RecordAsync(entry);
    }

    /// <summary>
    /// The response body has already been written by the time we get here, so
    /// the reason is derived from the status code rather than re-read from it.
    /// </summary>
    private static string ReasonFor(int status) => status switch
    {
        400 => "Solicitud inválida",
        401 => "Credenciales inválidas o sesión expirada",
        403 => "Permisos insuficientes",
        404 => "Recurso no encontrado",
        408 => "Tiempo de espera agotado",
        409 => "Conflicto con el estado actual",
        422 => "Validación fallida",
        >= 500 => "Error interno del servidor",
        _ => $"Error HTTP {status}"
    };

    private static string Truncate(string value, int max) =>
        value.Length <= max ? value : value[..max];
}
