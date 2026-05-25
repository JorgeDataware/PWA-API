using Microsoft.EntityFrameworkCore;
using PWA_API.Domain.Entities;
using PWA_API.Domain.Enums;

namespace PWA_API.Infrastructure.Persistence;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (await context.Users.AnyAsync()) return;

        var admin = new User
        {
            FullName = "Admin User",
            Username = "admin",
            Email = "admin@pwa-news.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin1234!"),
            Role = UserRole.Admin,
            CreatedAt = DateTime.UtcNow
        };

        var user = new User
        {
            FullName = "Regular User",
            Username = "usuario",
            Email = "user@pwa-news.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("User1234!"),
            Role = UserRole.User,
            CreatedAt = DateTime.UtcNow
        };

        context.Users.AddRange(admin, user);
        await context.SaveChangesAsync();

        var news = new[]
        {
            new News
            {
                Title = "Lanzamiento de .NET 10: Lo que debes saber",
                AuthorId = admin.Id,
                Content = "Microsoft ha lanzado oficialmente .NET 10, trayendo mejoras de rendimiento significativas, nuevas APIs y mejor soporte para IA generativa...",
                PublishedAt = DateTime.UtcNow.AddDays(-2),
                ImageUrl = "https://example.com/dotnet10.jpg"
            },
            new News
            {
                Title = "Apple Vision Pro 2: ¿El futuro de la computación espacial?",
                AuthorId = admin.Id,
                Content = "Apple ha revelado la segunda generación de su visor de realidad mixta Vision Pro, con mejoras en el procesador, batería y una interfaz renovada...",
                PublishedAt = DateTime.UtcNow.AddDays(-1),
                ImageUrl = "https://example.com/visionpro2.jpg"
            },
            new News
            {
                Title = "ChatGPT-5 supera todos los benchmarks conocidos",
                AuthorId = admin.Id,
                Content = "OpenAI presentó GPT-5, su modelo de lenguaje más avanzado hasta la fecha, que logra resultados sin precedentes en razonamiento y programación...",
                PublishedAt = DateTime.UtcNow,
                ImageUrl = "https://example.com/gpt5.jpg"
            }
        };

        context.News.AddRange(news);
        await context.SaveChangesAsync();
    }
}
