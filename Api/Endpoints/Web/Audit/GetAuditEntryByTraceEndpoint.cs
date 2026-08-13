using FastEndpoints;
using PWA_API.Application.DTOs.Audit;
using PWA_API.Application.Interfaces.Services;

namespace PWA_API.Api.Endpoints.Web.Audit;

public class GetAuditEntryByTraceRequest
{
    public string TraceId { get; set; } = string.Empty;
}

/// <summary>
/// Lookup by the code the client shows when a request fails ("código de
/// rastreo"), so a reported failure can be investigated without searching logs
/// by hand.
/// </summary>
public class GetAuditEntryByTraceEndpoint(IAuditService auditService)
    : Endpoint<GetAuditEntryByTraceRequest, AuditLogDto>
{
    public override void Configure()
    {
        Get("/api/web/audit/{traceId}");
        Roles("Admin");
        Tags("Web - Audit");
        Summary(s => s.Summary = "[Web] Get one audit entry by its trace id");
    }

    public override async Task HandleAsync(GetAuditEntryByTraceRequest req, CancellationToken ct)
    {
        var result = await auditService.GetByTraceIdAsync(req.TraceId);
        if (!result.IsSuccess)
        {
            await SendStringAsync(result.Error!, result.StatusCode, cancellation: ct);
            return;
        }
        await SendAsync(result.Value!, 200, ct);
    }
}
