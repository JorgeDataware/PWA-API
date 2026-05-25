using FastEndpoints;
using PWA_API.Application.DTOs.News;
using PWA_API.Application.Interfaces.Services;

namespace PWA_API.Api.Endpoints.Wearable.News;

public class GetNewsByIdWearableRequest { public int Id { get; set; } }

public class GetNewsByIdWearableEndpoint(INewsService newsService) : Endpoint<GetNewsByIdWearableRequest, NewsWearableDto>
{
    public override void Configure()
    {
        Get("/api/wearable/news/{id}");
        Roles("Admin", "User");
        Tags("Wearable - News");
        Summary(s =>
        {
            s.Summary = "[Wearable] Get news by ID";
            s.Description = "Returns a single news article in lightweight format for wearable devices.";
        });
    }

    public override async Task HandleAsync(GetNewsByIdWearableRequest req, CancellationToken ct)
    {
        var result = await newsService.GetByIdWearableAsync(req.Id);
        if (!result.IsSuccess)
        {
            await SendAsync(null!, result.StatusCode, ct);
            return;
        }
        await SendAsync(result.Value!, 200, ct);
    }
}
