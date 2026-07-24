using FastEndpoints;
using PWA_API.Application.DTOs.Users;
using PWA_API.Application.Interfaces.Services;

namespace PWA_API.Api.Endpoints.Web.Users;

public class UpdateUserRequest
{
    public int Id { get; set; }
    public string? FullName { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public int? Role { get; set; }
    public bool? MustChangePassword { get; set; }
}

public class UpdateUserEndpoint(IUserService userService) : Endpoint<UpdateUserRequest, UserDto>
{
    public override void Configure()
    {
        Put("/api/web/users/{id}");
        Roles("Admin");
        Tags("Web - Users");
        Summary(s => s.Summary = "[Web] Update user");
    }

    public override async Task HandleAsync(UpdateUserRequest req, CancellationToken ct)
    {
        var dto = new UpdateUserDto(req.FullName, req.Username, req.Email, req.Password, req.Role, req.MustChangePassword);
        var result = await userService.UpdateAsync(req.Id, dto);
        if (!result.IsSuccess)
        {
            await SendAsync(null!, result.StatusCode, ct);
            return;
        }
        await SendAsync(result.Value!, 200, ct);
    }
}
