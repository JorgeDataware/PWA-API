using PWA_API.Application.DTOs.Auth;
using PWA_API.Domain.Common;

namespace PWA_API.Application.Interfaces.Services;

public interface IAuthService
{
    Task<Result<LoginResponse>> LoginAsync(LoginRequest request);
    Task<Result<LoginResponse>> RegisterAsync(RegisterRequest request);
}
