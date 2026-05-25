using FastEndpoints;
using PWA_API.Application.DTOs.News;
using PWA_API.Application.Interfaces.Services;

namespace PWA_API.Api.Endpoints.Web.News;

public class UpdateNewsRequest
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public string? ImageUrl { get; set; }
}

public class UpdateNewsEndpoint(INewsService newsService) : Endpoint<UpdateNewsRequest, NewsDto>
{
    public override void Configure()
    {
        Put("/api/web/news/{id}");
        Roles("Admin");
        Tags("Web - News");
        Summary(s => s.Summary = "[Web] Update news");
    }

    public override async Task HandleAsync(UpdateNewsRequest req, CancellationToken ct)
    {
        var dto = new UpdateNewsDto(req.Title, req.Content, req.ImageUrl);
        var result = await newsService.UpdateAsync(req.Id, dto);
        if (!result.IsSuccess)
        {
            await SendAsync(null!, result.StatusCode, ct);
            return;
        }
        await SendAsync(result.Value!, 200, ct);
    }
}
