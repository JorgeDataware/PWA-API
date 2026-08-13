using FastEndpoints;
using PWA_API.Application.DTOs.Audit;
using PWA_API.Application.Interfaces.Services;

namespace PWA_API.Api.Endpoints.Web.Audit;

public class GetAuditLogRequest
{
    /// <summary>Maximum number of entries to return (1–200, default 50).</summary>
    public int Limit { get; set; } = 50;

    /// <summary>When true, only failed operations are returned.</summary>
    public bool OnlyFailures { get; set; }
}

public class GetAuditLogEndpoint(IAuditService auditService)
    : Endpoint<GetAuditLogRequest, IEnumerable<AuditLogDto>>
{
    public override void Configure()
    {
        Get("/api/web/audit");
        Roles("Admin");
        Tags("Web - Audit");
        Summary(s =>
        {
            s.Summary = "[Web] Get the audit trail";
            s.Description = "Registro de operaciones que modificaron estado, exitosas o fallidas, " +
                            "en orden cronológico inverso. Sólo administradores.";
        });
    }

    public override async Task HandleAsync(GetAuditLogRequest req, CancellationToken ct)
    {
        var result = await auditService.GetRecentAsync(req.Limit, req.OnlyFailures);
        await SendAsync(result.Value!, 200, ct);
    }
}
