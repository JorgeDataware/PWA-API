using PWA_API.Domain.Entities;

namespace PWA_API.Application.Interfaces.Repositories;

public interface INewsRepository
{
    Task<News?> GetByIdAsync(int id);
    Task AddAsync(News news);
    Task UpdateAsync(News news);
    Task DeleteAsync(News news);
}
