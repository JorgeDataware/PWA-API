using PWA_API.Application.Interfaces.Repositories;
using PWA_API.Domain.Entities;

namespace PWA_API.Infrastructure.Persistence.Repositories;

public class NewsRepository(AppDbContext context) : INewsRepository
{
    public Task<News?> GetByIdAsync(int id) =>
        context.News.FindAsync(id).AsTask();

    public async Task AddAsync(News news)
    {
        context.News.Add(news);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(News news)
    {
        context.News.Update(news);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(News news)
    {
        context.News.Remove(news);
        await context.SaveChangesAsync();
    }
}
