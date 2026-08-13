namespace PWA_API.Domain.Entities;

/// <summary>
/// Immutable record of an operation that changed state (or tried to).
/// Written for every non-GET request, successful or not, so a failure can be
/// reconstructed afterwards: who did what, when, from where, and how it ended.
/// The <see cref="TraceId"/> is the same identifier returned to the client in
/// the <c>X-Trace-Id</c> header and written to the application log, which is
/// what links a user-reported error to its log entry.
/// </summary>
public class AuditLog
{
    public long Id { get; set; }

    public DateTime OccurredAt { get; set; } = DateTime.UtcNow;

    /// <summary>Correlation id shared with the HTTP response and the log file.</summary>
    public string TraceId { get; set; } = string.Empty;

    /// <summary>Null for anonymous requests, e.g. a failed login attempt.</summary>
    public int? UserId { get; set; }

    public string? Username { get; set; }

    /// <summary>Role of the actor at the time of the action.</summary>
    public string? Role { get; set; }

    public string Method { get; set; } = string.Empty;

    public string Path { get; set; } = string.Empty;

    /// <summary>Short description of the operation, e.g. "Deshabilitar usuario".</summary>
    public string Action { get; set; } = string.Empty;

    public int StatusCode { get; set; }

    public bool Success { get; set; }

    public int DurationMs { get; set; }

    public string? IpAddress { get; set; }

    /// <summary>Error message when the operation failed; null on success.</summary>
    public string? Error { get; set; }
}
