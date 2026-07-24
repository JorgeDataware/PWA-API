namespace PWA_API.Application.DTOs.Users;

public record CreateUserDto(
    string FullName,
    string Username,
    string Email,
    string Password,
    int Role,
    bool MustChangePassword = false
);
