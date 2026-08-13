using PWA_API.Domain.Entities;

namespace PWA_API.Application.Interfaces.Repositories;

public interface IAuditLogRepository
{
    Task AddAsync(AuditLog entry);

    /// <summary>
    /// Most recent entries first. When <paramref name="onlyFailures"/> is true
    /// only unsuccessful operations are returned — the "qué falló" view.
    /// </summary>
    Task<IReadOnlyList<AuditLog>> GetRecentAsync(int limit, bool onlyFailures);

    Task<AuditLog?> GetByTraceIdAsync(string traceId);
}
