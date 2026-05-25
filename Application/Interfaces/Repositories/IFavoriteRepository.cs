using PWA_API.Domain.Entities;

namespace PWA_API.Application.Interfaces.Repositories;

public interface IFavoriteRepository
{
    Task<Favorite?> GetByUserAndNewsAsync(int userId, int newsId);
    Task AddAsync(Favorite favorite);
    Task DeleteAsync(Favorite favorite);
}
