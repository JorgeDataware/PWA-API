using FastEndpoints;
using PWA_API.Api.Extensions;
using PWA_API.Application.DTOs.Favorites;
using PWA_API.Application.Interfaces.Services;

namespace PWA_API.Api.Endpoints.Favorites;

public class AddFavoriteRequest { public int NewsId { get; set; } }

public class AddFavoriteEndpoint(IFavoriteService favoriteService) : Endpoint<AddFavoriteRequest, FavoriteDto>
{
    public override void Configure()
    {
        Post("/api/favorites");
        Roles("Admin", "User");
        Tags("Favorites");
        Summary(s =>
        {
            s.Summary = "Add to favorites";
            s.Description = "Adds a news article to the authenticated user's favorites.";
        });
    }

    public override async Task HandleAsync(AddFavoriteRequest req, CancellationToken ct)
    {
        var userId = User.GetUserId();
        var result = await favoriteService.AddAsync(userId, req.NewsId);
        if (!result.IsSuccess)
        {
            await SendAsync(null!, result.StatusCode, ct);
            return;
        }
        await SendAsync(result.Value!, result.StatusCode, ct);
    }
}
