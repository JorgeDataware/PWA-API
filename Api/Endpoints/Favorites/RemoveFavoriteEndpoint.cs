using FastEndpoints;
using PWA_API.Api.Extensions;
using PWA_API.Application.Interfaces.Services;

namespace PWA_API.Api.Endpoints.Favorites;

public class RemoveFavoriteRequest { public int NewsId { get; set; } }

public class RemoveFavoriteEndpoint(IFavoriteService favoriteService) : Endpoint<RemoveFavoriteRequest>
{
    public override void Configure()
    {
        Delete("/api/favorites/{newsId}");
        Roles("Admin", "User");
        Tags("Favorites");
        Summary(s =>
        {
            s.Summary = "Remove from favorites";
            s.Description = "Removes a news article from the authenticated user's favorites.";
        });
    }

    public override async Task HandleAsync(RemoveFavoriteRequest req, CancellationToken ct)
    {
        var userId = User.GetUserId();
        var result = await favoriteService.RemoveAsync(userId, req.NewsId);
        if (!result.IsSuccess)
        {
            await SendStringAsync(result.Error!, result.StatusCode, cancellation: ct);
            return;
        }
        await SendNoContentAsync(ct);
    }
}
