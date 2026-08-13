using PWA_API.Application.DTOs.Audit;
using PWA_API.Domain.Common;
using PWA_API.Domain.Entities;

namespace PWA_API.Application.Interfaces.Services;

public interface IAuditService
{
    Task RecordAsync(AuditLog entry);

    Task<Result<IEnumerable<AuditLogDto>>> GetRecentAsync(int limit = 50, bool onlyFailures = false);

    /// <summary>
    /// Looks up the operation behind a trace id — the code the client shows to
    /// the user when a request fails.
    /// </summary>
    Task<Result<AuditLogDto>> GetByTraceIdAsync(string traceId);
}
