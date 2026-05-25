using FastEndpoints;
using PWA_API.Application.DTOs.Users;
using PWA_API.Application.Interfaces.Services;

namespace PWA_API.Api.Endpoints.Web.Users;

public class GetUsersEndpoint(IUserService userService) : EndpointWithoutRequest<IEnumerable<UserDto>>
{
    public override void Configure()
    {
        Get("/api/web/users");
        Roles("Admin");
        Tags("Web - Users");
        Summary(s =>
        {
            s.Summary = "[Web] Get all users";
            s.Description = "Returns all registered users. Admin only.";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await userService.GetAllAsync();
        await SendAsync(result.Value!, 200, ct);
    }
}
