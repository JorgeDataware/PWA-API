using PWA_API.Application.DTOs.Audit;
using PWA_API.Application.Interfaces.Repositories;
using PWA_API.Application.Interfaces.Services;
using PWA_API.Domain.Common;
using PWA_API.Domain.Entities;

namespace PWA_API.Application.Services;

public class AuditService(IAuditLogRepository repository) : IAuditService
{
    private const int MaxLimit = 200;

    public Task RecordAsync(AuditLog entry) => repository.AddAsync(entry);

    public async Task<Result<IEnumerable<AuditLogDto>>> GetRecentAsync(
        int limit = 50, bool onlyFailures = false)
    {
        if (limit <= 0) limit = 50;
        if (limit > MaxLimit) limit = MaxLimit;

        var entries = await repository.GetRecentAsync(limit, onlyFailures);
        return Result<IEnumerable<AuditLogDto>>.Success(entries.Select(Map));
    }

    public async Task<Result<AuditLogDto>> GetByTraceIdAsync(string traceId)
    {
        if (string.IsNullOrWhiteSpace(traceId))
            return Result<AuditLogDto>.Failure("El código de rastreo es obligatorio.");

        var entry = await repository.GetByTraceIdAsync(traceId.Trim());
        return entry is null
            ? Result<AuditLogDto>.NotFound($"No hay ninguna operación registrada con el código {traceId}.")
            : Result<AuditLogDto>.Success(Map(entry));
    }

    /// <summary>
    /// Human-readable name for an operation, derived from its method and route
    /// (e.g. <c>PATCH /api/web/users/3/status</c> → "Cambio de estado de cuenta").
    /// Route ids are ignored so the same operation always gets the same name.
    /// </summary>
    public static string DescribeAction(string method, string path)
    {
        var route = path.ToLowerInvariant();

        if (route.StartsWith("/api/auth/login")) return "Inicio de sesión";
        if (route.StartsWith("/api/auth/register")) return "Registro de cuenta";
        if (route.StartsWith("/api/profile")) return "Actualización de perfil propio";

        if (route.StartsWith("/api/web/users"))
        {
            if (route.EndsWith("/status")) return "Cambio de estado de cuenta";
            return method switch
            {
                "POST" => "Alta de usuario",
                "PUT" => "Edición de usuario",
                "DELETE" => "Baja de usuario",
                _ => $"Operación sobre usuarios ({method})"
            };
        }

        if (route.StartsWith("/api/web/news"))
        {
            return method switch
            {
                "POST" => "Publicación de noticia",
                "PUT" => "Edición de noticia",
                "DELETE" => "Eliminación de noticia",
                _ => $"Operación sobre noticias ({method})"
            };
        }

        if (route.StartsWith("/api/web/favorites"))
        {
            return method switch
            {
                "POST" => "Alta de favorito",
                "DELETE" => "Baja de favorito",
                _ => $"Operación sobre favoritos ({method})"
            };
        }

        return $"{method} {path}";
    }

    private static AuditLogDto Map(AuditLog a) => new()
    {
        Id = a.Id,
        OccurredAt = a.OccurredAt,
        TraceId = a.TraceId,
        UserId = a.UserId,
        Username = a.Username,
        Role = a.Role,
        Method = a.Method,
        Path = a.Path,
        Action = a.Action,
        StatusCode = a.StatusCode,
        Success = a.Success,
        DurationMs = a.DurationMs,
        IpAddress = a.IpAddress,
        Error = a.Error
    };
}
