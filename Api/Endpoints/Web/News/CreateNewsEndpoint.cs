using FastEndpoints;
using PWA_API.Api.Extensions;
using PWA_API.Application.DTOs.News;
using PWA_API.Application.Interfaces.Services;
using PWA_API.Application.Validators;

namespace PWA_API.Api.Endpoints.Web.News;

public class CreateNewsEndpoint(INewsService newsService) : Endpoint<CreateNewsDto, NewsDto>
{
    public override void Configure()
    {
        Post("/api/web/news");
        Roles("Admin");
        Validator<CreateNewsValidator>();
        Tags("Web - News");
        Summary(s =>
        {
            s.Summary = "[Web] Create news";
            s.Description = "Creates a new article. Admin only.";
        });
    }

    public override async Task HandleAsync(CreateNewsDto req, CancellationToken ct)
    {
        var authorId = User.GetUserId();
        var result = await newsService.CreateAsync(authorId, req);
        if (!result.IsSuccess)
        {
            await SendAsync(null!, result.StatusCode, ct);
            return;
        }
        await SendAsync(result.Value!, result.StatusCode, ct);
    }
}
