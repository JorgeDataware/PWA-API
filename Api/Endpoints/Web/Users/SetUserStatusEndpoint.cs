using FastEndpoints;
using PWA_API.Api.Extensions;
using PWA_API.Application.DTOs.Users;
using PWA_API.Application.Interfaces.Services;

namespace PWA_API.Api.Endpoints.Web.Users;

public class SetUserStatusRequest
{
    public int Id { get; set; }
    public bool IsActive { get; set; }
}

public class SetUserStatusEndpoint(IUserService userService) : Endpoint<SetUserStatusRequest, UserDto>
{
    public override void Configure()
    {
        Patch("/api/web/users/{id}/status");
        Roles("Admin");
        Tags("Web - Users");
        Summary(s => s.Summary = "[Web] Enable or disable a user account");
    }

    public override async Task HandleAsync(SetUserStatusRequest req, CancellationToken ct)
    {
        var result = await userService.SetActiveAsync(req.Id, req.IsActive, User.GetUserId());
        if (!result.IsSuccess)
        {
            await SendStringAsync(result.Error!, result.StatusCode, cancellation: ct);
            return;
        }
        await SendAsync(result.Value!, 200, ct);
    }
}
