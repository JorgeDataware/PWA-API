using FastEndpoints;
using PWA_API.Application.Interfaces.Services;

namespace PWA_API.Api.Endpoints.Web.News;

public class DeleteNewsRequest { public int Id { get; set; } }

public class DeleteNewsEndpoint(INewsService newsService) : Endpoint<DeleteNewsRequest>
{
    public override void Configure()
    {
        Delete("/api/web/news/{id}");
        Roles("Admin");
        Tags("Web - News");
        Summary(s => s.Summary = "[Web] Delete news");
    }

    public override async Task HandleAsync(DeleteNewsRequest req, CancellationToken ct)
    {
        var result = await newsService.DeleteAsync(req.Id);
        if (!result.IsSuccess)
        {
            await SendStringAsync(result.Error!, result.StatusCode, cancellation: ct);
            return;
        }
        await SendNoContentAsync(ct);
    }
}
