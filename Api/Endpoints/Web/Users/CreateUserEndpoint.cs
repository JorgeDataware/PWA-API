using FastEndpoints;
using PWA_API.Application.DTOs.Users;
using PWA_API.Application.Interfaces.Services;
using PWA_API.Application.Validators;

namespace PWA_API.Api.Endpoints.Web.Users;

public class CreateUserEndpoint(IUserService userService) : Endpoint<CreateUserDto, UserDto>
{
    public override void Configure()
    {
        Post("/api/web/users");
        Roles("Admin");
        Validator<CreateUserValidator>();
        Tags("Web - Users");
        Summary(s =>
        {
            s.Summary = "[Web] Create user";
            s.Description = "Creates a new user. Admin only.";
        });
    }

    public override async Task HandleAsync(CreateUserDto req, CancellationToken ct)
    {
        var result = await userService.CreateAsync(req);
        if (!result.IsSuccess)
        {
            await SendAsync(null!, result.StatusCode, ct);
            return;
        }
        await SendAsync(result.Value!, result.StatusCode, ct);
    }
}
