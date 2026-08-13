namespace PWA_API.Application.DTOs.Audit;

public class AuditLogDto
{
    public long Id { get; set; }
    public DateTime OccurredAt { get; set; }
    public string TraceId { get; set; } = string.Empty;
    public int? UserId { get; set; }
    public string? Username { get; set; }
    public string? Role { get; set; }
    public string Method { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public int StatusCode { get; set; }
    public bool Success { get; set; }
    public int DurationMs { get; set; }
    public string? IpAddress { get; set; }
    public string? Error { get; set; }
}
