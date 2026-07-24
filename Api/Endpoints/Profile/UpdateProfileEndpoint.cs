using FastEndpoints;
using PWA_API.Api.Extensions;
using PWA_API.Application.DTOs.Users;
using PWA_API.Application.Interfaces.Services;

namespace PWA_API.Api.Endpoints.Profile;

public class UpdateProfileRequest
{
    public string? FullName { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
}

public class UpdateProfileEndpoint(IUserService userService) : Endpoint<UpdateProfileRequest, UserDto>
{
    public override void Configure()
    {
        Put("/api/profile");
        Roles("Admin", "User", "Guest");
        Tags("Profile");
        Summary(s =>
        {
            s.Summary = "Update my profile";
            s.Description = "Updates the authenticated user's profile. Cannot change role.";
        });
    }

    public override async Task HandleAsync(UpdateProfileRequest req, CancellationToken ct)
    {
        var userId = User.GetUserId();
        var dto = new UpdateUserDto(req.FullName, req.Username, req.Email, req.Password, null);
        var result = await userService.UpdateAsync(userId, dto);
        if (!result.IsSuccess)
        {
            await SendAsync(null!, result.StatusCode, ct);
            return;
        }
        await SendAsync(result.Value!, 200, ct);
    }
}
