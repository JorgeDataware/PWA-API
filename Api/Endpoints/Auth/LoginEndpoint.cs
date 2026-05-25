using FastEndpoints;
using PWA_API.Application.DTOs.Auth;
using PWA_API.Application.Interfaces.Services;
using PWA_API.Application.Validators;

namespace PWA_API.Api.Endpoints.Auth;

public class LoginEndpoint(IAuthService authService) : Endpoint<LoginRequest, LoginResponse>
{
    public override void Configure()
    {
        Post("/api/auth/login");
        AllowAnonymous();
        Validator<LoginValidator>();
        Tags("Auth");
        Summary(s =>
        {
            s.Summary = "Login";
            s.Description = "Authenticate a user and return a JWT token.";
        });
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        var result = await authService.LoginAsync(req);
        if (!result.IsSuccess)
        {
            await SendAsync(null!, result.StatusCode, ct);
            return;
        }
        await SendAsync(result.Value!, result.StatusCode, ct);
    }
}
