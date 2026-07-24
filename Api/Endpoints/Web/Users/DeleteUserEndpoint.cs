using FastEndpoints;
using PWA_API.Application.Interfaces.Services;
using PWA_API.Api.Extensions;

namespace PWA_API.Api.Endpoints.Web.Users;

public class DeleteUserRequest { public int Id { get; set; } }

public class DeleteUserEndpoint(IUserService userService) : Endpoint<DeleteUserRequest>
{
    public override void Configure()
    {
        Delete("/api/web/users/{id}");
        Roles("Admin");
        Tags("Web - Users");
        Summary(s => s.Summary = "[Web] Logically delete (deactivate) a user");
    }

    public override async Task HandleAsync(DeleteUserRequest req, CancellationToken ct)
    {
        var result = await userService.DeleteAsync(req.Id, User.GetUserId());
        if (!result.IsSuccess)
        {
            await SendStringAsync(result.Error!, result.StatusCode, cancellation: ct);
            return;
        }
        await SendNoContentAsync(ct);
    }
}
