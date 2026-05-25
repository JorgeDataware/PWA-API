namespace PWA_API.Application.DTOs.Auth;

public record RegisterRequest(
    string FullName,
    string Username,
    string Email,
    string Password
);
