using FastEndpoints;
using PWA_API.Application.DTOs.Auth;
using PWA_API.Application.Interfaces.Services;
using PWA_API.Application.Validators;

namespace PWA_API.Api.Endpoints.Auth;

public class RegisterEndpoint(IAuthService authService) : Endpoint<RegisterRequest, LoginResponse>
{
    public override void Configure()
    {
        Post("/api/auth/register");
        AllowAnonymous();
        Validator<RegisterValidator>();
        Tags("Auth");
        Summary(s =>
        {
            s.Summary = "Register";
            s.Description = "Register a new user (role: User by default).";
        });
    }

    public override async Task HandleAsync(RegisterRequest req, CancellationToken ct)
    {
        var result = await authService.RegisterAsync(req);
        if (!result.IsSuccess)
        {
            await SendAsync(null!, result.StatusCode, ct);
            return;
        }
        await SendAsync(result.Value!, result.StatusCode, ct);
    }
}
