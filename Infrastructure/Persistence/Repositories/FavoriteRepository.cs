using Microsoft.EntityFrameworkCore;
using PWA_API.Application.Interfaces.Repositories;
using PWA_API.Domain.Entities;

namespace PWA_API.Infrastructure.Persistence.Repositories;

public class FavoriteRepository(AppDbContext context) : IFavoriteRepository
{
    public Task<Favorite?> GetByUserAndNewsAsync(int userId, int newsId) =>
        context.Favorites.FirstOrDefaultAsync(f => f.UserId == userId && f.NewsId == newsId);

    public async Task AddAsync(Favorite favorite)
    {
        context.Favorites.Add(favorite);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Favorite favorite)
    {
        context.Favorites.Remove(favorite);
        await context.SaveChangesAsync();
    }
}
