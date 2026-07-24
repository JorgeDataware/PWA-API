using PWA_API.Domain.Enums;

namespace PWA_API.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.User;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAt { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime? DeactivatedAt { get; set; }
    public bool MustChangePassword { get; set; }

    public ICollection<News> AuthoredNews { get; set; } = [];
    public ICollection<Favorite> Favorites { get; set; } = [];
}
