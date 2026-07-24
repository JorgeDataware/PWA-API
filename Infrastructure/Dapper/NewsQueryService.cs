using Dapper;
using PWA_API.Application.DTOs.Favorites;
using PWA_API.Application.DTOs.News;
using PWA_API.Application.DTOs.Users;
using PWA_API.Application.Interfaces.Dapper;
using PWA_API.Application.Interfaces.Services;

namespace PWA_API.Infrastructure.Dapper;

public class NewsQueryService(IDbConnectionFactory connectionFactory) : INewsQueryService
{
    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        const string sql = """
            SELECT u.id, u.full_name, u.username, u.email,
                   CASE u.role WHEN 1 THEN 'Admin' WHEN 2 THEN 'User' WHEN 3 THEN 'Guest' ELSE 'User' END AS role,
                   u.created_at, u.last_login_at, u.is_active, u.must_change_password
            FROM users u
            ORDER BY u.id
            """;
        using var conn = connectionFactory.CreateConnection();
        return await conn.QueryAsync<UserDto>(sql);
    }

    public async Task<UserDto?> GetUserByIdAsync(int id)
    {
        const string sql = """
            SELECT u.id, u.full_name, u.username, u.email,
                   CASE u.role WHEN 1 THEN 'Admin' WHEN 2 THEN 'User' WHEN 3 THEN 'Guest' ELSE 'User' END AS role,
                   u.created_at, u.last_login_at, u.is_active, u.must_change_password
            FROM users u
            WHERE u.id = @Id
            """;
        using var conn = connectionFactory.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<UserDto>(sql, new { Id = id });
    }

    public async Task<IEnumerable<NewsDto>> GetAllNewsAsync()
    {
        const string sql = """
            SELECT n.id, n.title, n.author_id, u.full_name AS author_name,
                   n.content, n.published_at, n.image_url
            FROM news n
            INNER JOIN users u ON n.author_id = u.id
            ORDER BY n.published_at DESC
            """;
        using var conn = connectionFactory.CreateConnection();
        return await conn.QueryAsync<NewsDto>(sql);
    }

    public async Task<NewsDto?> GetNewsByIdAsync(int id)
    {
        const string sql = """
            SELECT n.id, n.title, n.author_id, u.full_name AS author_name,
                   n.content, n.published_at, n.image_url
            FROM news n
            INNER JOIN users u ON n.author_id = u.id
            WHERE n.id = @Id
            """;
        using var conn = connectionFactory.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<NewsDto>(sql, new { Id = id });
    }

    public async Task<IEnumerable<NewsWearableDto>> GetAllNewsWearableAsync()
    {
        const string sql = """
            SELECT n.id, n.title, u.full_name AS author_name,
                   n.published_at, n.image_url
            FROM news n
            INNER JOIN users u ON n.author_id = u.id
            ORDER BY n.published_at DESC
            """;
        using var conn = connectionFactory.CreateConnection();
        return await conn.QueryAsync<NewsWearableDto>(sql);
    }

    public async Task<NewsWearableDto?> GetNewsByIdWearableAsync(int id)
    {
        const string sql = """
            SELECT n.id, n.title, u.full_name AS author_name,
                   n.published_at, n.image_url
            FROM news n
            INNER JOIN users u ON n.author_id = u.id
            WHERE n.id = @Id
            """;
        using var conn = connectionFactory.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<NewsWearableDto>(sql, new { Id = id });
    }

    public async Task<IEnumerable<FavoriteDto>> GetFavoritesByUserAsync(int userId)
    {
        const string sql = """
            SELECT f.id, f.news_id, n.title AS news_title,
                   n.image_url AS news_image_url, f.added_at
            FROM favorites f
            INNER JOIN news n ON f.news_id = n.id
            WHERE f.user_id = @UserId
            ORDER BY f.added_at DESC
            """;
        using var conn = connectionFactory.CreateConnection();
        return await conn.QueryAsync<FavoriteDto>(sql, new { UserId = userId });
    }
}
