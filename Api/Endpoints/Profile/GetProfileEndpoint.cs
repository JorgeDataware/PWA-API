using FastEndpoints;
using PWA_API.Api.Extensions;
using PWA_API.Application.DTOs.Users;
using PWA_API.Application.Interfaces.Services;

namespace PWA_API.Api.Endpoints.Profile;

public class GetProfileEndpoint(IUserService userService) : EndpointWithoutRequest<UserDto>
{
    public override void Configure()
    {
        Get("/api/profile");
        Roles("Admin", "User", "Guest");
        Tags("Profile");
        Summary(s =>
        {
            s.Summary = "Get my profile";
            s.Description = "Returns the authenticated user's profile data.";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = User.GetUserId();
        var result = await userService.GetByIdAsync(userId);
        if (!result.IsSuccess)
        {
            await SendAsync(null!, result.StatusCode, ct);
            return;
        }
        await SendAsync(result.Value!, 200, ct);
    }
}
