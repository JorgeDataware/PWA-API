using PWA_API.Application.DTOs.Auth;
using PWA_API.Application.Interfaces.Repositories;
using PWA_API.Application.Interfaces.Services;
using PWA_API.Domain.Common;
using PWA_API.Domain.Entities;
using PWA_API.Domain.Enums;

namespace PWA_API.Application.Services;

public class AuthService(IUserRepository userRepository, ITokenService tokenService) : IAuthService
{
    public async Task<Result<LoginResponse>> LoginAsync(LoginRequest request)
    {
        var user = await userRepository.GetByEmailAsync(request.Email);
        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return Result<LoginResponse>.Unauthorized("Invalid email or password.");

        return BuildLoginResponse(user);
    }

    public async Task<Result<LoginResponse>> RegisterAsync(RegisterRequest request)
    {
        if (await userRepository.ExistsByEmailAsync(request.Email))
            return Result<LoginResponse>.Failure("Email is already in use.");

        if (await userRepository.ExistsByUsernameAsync(request.Username))
            return Result<LoginResponse>.Failure("Username is already taken.");

        var user = new User
        {
            FullName = request.FullName,
            Username = request.Username,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = UserRole.User
        };

        await userRepository.AddAsync(user);
        return BuildLoginResponse(user, 201);
    }

    private Result<LoginResponse> BuildLoginResponse(User user, int statusCode = 200)
    {
        var token = tokenService.GenerateToken(user);
        var response = new LoginResponse(
            token,
            "Bearer",
            tokenService.GetExpiresIn(),
            new UserInfoDto(user.Id, user.FullName, user.Username, user.Email, user.Role.ToString())
        );
        return Result<LoginResponse>.Success(response, statusCode);
    }
}
