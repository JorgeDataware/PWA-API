using FastEndpoints;
using PWA_API.Application.DTOs.News;
using PWA_API.Application.Interfaces.Services;

namespace PWA_API.Api.Endpoints.Web.News;

public class GetNewsByIdRequest { public int Id { get; set; } }

public class GetNewsByIdEndpoint(INewsService newsService) : Endpoint<GetNewsByIdRequest, NewsDto>
{
    public override void Configure()
    {
        Get("/api/web/news/{id}");
        Roles("Admin", "User", "Guest");
        Tags("Web - News");
        Summary(s => s.Summary = "[Web] Get news by ID");
    }

    public override async Task HandleAsync(GetNewsByIdRequest req, CancellationToken ct)
    {
        var result = await newsService.GetByIdAsync(req.Id);
        if (!result.IsSuccess)
        {
            await SendAsync(null!, result.StatusCode, ct);
            return;
        }
        await SendAsync(result.Value!, 200, ct);
    }
}
