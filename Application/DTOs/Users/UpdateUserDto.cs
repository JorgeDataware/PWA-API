namespace PWA_API.Application.DTOs.Users;

public record UpdateUserDto(
    string? FullName,
    string? Username,
    string? Email,
    string? Password,
    int? Role,
    bool? MustChangePassword = null
);
