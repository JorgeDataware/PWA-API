using FastEndpoints;
using PWA_API.Application.DTOs.News;
using PWA_API.Application.Interfaces.Services;

namespace PWA_API.Api.Endpoints.Web.News;

public class SearchNewsRequest
{
    public string Q { get; set; } = string.Empty;
}

public class SearchNewsEndpoint(INewsService newsService) : Endpoint<SearchNewsRequest, IEnumerable<NewsDto>>
{
    public override void Configure()
    {
        Get("/api/web/news/search");
        AllowAnonymous();
        Tags("Web - News");
        Summary(s =>
        {
            s.Summary = "[Web] Search news";
            s.Description = "Internal search over title and content (case-insensitive substring match). " +
                             "Not a third-party search engine — query stays inside our own database.";
        });
    }

    public override async Task HandleAsync(SearchNewsRequest req, CancellationToken ct)
    {
        var result = await newsService.SearchAsync(req.Q);
        await SendAsync(result.Value!, 200, ct);
    }
}
