using PWA_API.Application.DTOs.Favorites;
using PWA_API.Application.DTOs.News;
using PWA_API.Application.DTOs.Users;

namespace PWA_API.Application.Interfaces.Services;

public interface INewsQueryService
{
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<UserDto?> GetUserByIdAsync(int id);
    Task<IEnumerable<NewsDto>> GetAllNewsAsync();
    Task<NewsDto?> GetNewsByIdAsync(int id);
    Task<IEnumerable<NewsWearableDto>> GetAllNewsWearableAsync();
    Task<NewsWearableDto?> GetNewsByIdWearableAsync(int id);
    Task<IEnumerable<FavoriteDto>> GetFavoritesByUserAsync(int userId);
}
