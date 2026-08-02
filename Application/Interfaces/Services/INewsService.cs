using PWA_API.Application.DTOs.News;
using PWA_API.Domain.Common;

namespace PWA_API.Application.Interfaces.Services;

public interface INewsService
{
    Task<Result<IEnumerable<NewsDto>>> GetAllAsync();
    Task<Result<NewsDto>> GetByIdAsync(int id);
    Task<Result<IEnumerable<NewsDto>>> SearchAsync(string query);
    Task<Result<IEnumerable<NewsWearableDto>>> GetAllWearableAsync();
    Task<Result<NewsWearableDto>> GetByIdWearableAsync(int id);
    Task<Result<NewsDto>> CreateAsync(int authorId, CreateNewsDto dto);
    Task<Result<NewsDto>> UpdateAsync(int id, UpdateNewsDto dto);
    Task<Result<bool>> DeleteAsync(int id);
}
