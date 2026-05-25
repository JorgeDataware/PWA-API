using FastEndpoints;
using PWA_API.Api.Extensions;
using PWA_API.Application.DTOs.Favorites;
using PWA_API.Application.Interfaces.Services;

namespace PWA_API.Api.Endpoints.Favorites;

public class GetFavoritesEndpoint(IFavoriteService favoriteService) : EndpointWithoutRequest<IEnumerable<FavoriteDto>>
{
    public override void Configure()
    {
        Get("/api/favorites");
        Roles("Admin", "User");
        Tags("Favorites");
        Summary(s =>
        {
            s.Summary = "Get my favorites";
            s.Description = "Returns the authenticated user's favorite news list.";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = User.GetUserId();
        var result = await favoriteService.GetByUserAsync(userId);
        await SendAsync(result.Value!, 200, ct);
    }
}
