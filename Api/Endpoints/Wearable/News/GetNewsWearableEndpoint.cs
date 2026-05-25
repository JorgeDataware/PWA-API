using FastEndpoints;
using PWA_API.Application.DTOs.News;
using PWA_API.Application.Interfaces.Services;

namespace PWA_API.Api.Endpoints.Wearable.News;

public class GetNewsWearableEndpoint(INewsService newsService) : EndpointWithoutRequest<IEnumerable<NewsWearableDto>>
{
    public override void Configure()
    {
        Get("/api/wearable/news");
        Roles("Admin", "User");
        Tags("Wearable - News");
        Summary(s =>
        {
            s.Summary = "[Wearable] Get all news";
            s.Description = "Returns a lightweight list of news articles optimized for wearable devices.";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await newsService.GetAllWearableAsync();
        await SendAsync(result.Value!, 200, ct);
    }
}
