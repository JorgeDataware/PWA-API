namespace PWA_API.Application.DTOs.Auth;

public record LoginResponse(
    string Token,
    string TokenType,
    int ExpiresIn,
    UserInfoDto User
);

public record UserInfoDto(int Id, string FullName, string Username, string Email, string Role);
