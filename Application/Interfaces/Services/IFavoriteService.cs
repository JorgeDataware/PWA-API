using PWA_API.Application.DTOs.Favorites;
using PWA_API.Domain.Common;

namespace PWA_API.Application.Interfaces.Services;

public interface IFavoriteService
{
    Task<Result<IEnumerable<FavoriteDto>>> GetByUserAsync(int userId);
    Task<Result<FavoriteDto>> AddAsync(int userId, int newsId);
    Task<Result<bool>> RemoveAsync(int userId, int newsId);
}
