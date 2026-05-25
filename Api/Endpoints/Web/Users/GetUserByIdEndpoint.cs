using FastEndpoints;
using PWA_API.Application.DTOs.Users;
using PWA_API.Application.Interfaces.Services;

namespace PWA_API.Api.Endpoints.Web.Users;

public class GetUserByIdRequest { public int Id { get; set; } }

public class GetUserByIdEndpoint(IUserService userService) : Endpoint<GetUserByIdRequest, UserDto>
{
    public override void Configure()
    {
        Get("/api/web/users/{id}");
        Roles("Admin");
        Tags("Web - Users");
        Summary(s => s.Summary = "[Web] Get user by ID");
    }

    public override async Task HandleAsync(GetUserByIdRequest req, CancellationToken ct)
    {
        var result = await userService.GetByIdAsync(req.Id);
        if (!result.IsSuccess)
        {
            await SendAsync(null!, result.StatusCode, ct);
            return;
        }
        await SendAsync(result.Value!, 200, ct);
    }
}
