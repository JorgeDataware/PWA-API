using FastEndpoints;
using PWA_API.Application.DTOs.News;
using PWA_API.Application.Interfaces.Services;

namespace PWA_API.Api.Endpoints.Web.News;

public class GetNewsEndpoint(INewsService newsService) : EndpointWithoutRequest<IEnumerable<NewsDto>>
{
    public override void Configure()
    {
        Get("/api/web/news");
        Roles("Admin", "User");
        Tags("Web - News");
        Summary(s =>
        {
            s.Summary = "[Web] Get all news";
            s.Description = "Returns all published news articles with full content.";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await newsService.GetAllAsync();
        await SendAsync(result.Value!, 200, ct);
    }
}
