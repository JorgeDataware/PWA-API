using PWA_API.Application.DTOs.News;
using PWA_API.Application.Interfaces.Repositories;
using PWA_API.Application.Interfaces.Services;
using PWA_API.Domain.Common;
using PWA_API.Domain.Entities;

namespace PWA_API.Application.Services;

public class NewsService(
    INewsRepository newsRepository,
    INewsQueryService queryService) : INewsService
{
    public async Task<Result<IEnumerable<NewsDto>>> GetAllAsync()
    {
        var news = await queryService.GetAllNewsAsync();
        return Result<IEnumerable<NewsDto>>.Success(news);
    }

    public async Task<Result<NewsDto>> GetByIdAsync(int id)
    {
        var news = await queryService.GetNewsByIdAsync(id);
        if (news is null)
            return Result<NewsDto>.NotFound($"News with id {id} not found.");
        return Result<NewsDto>.Success(news);
    }

    public async Task<Result<IEnumerable<NewsWearableDto>>> GetAllWearableAsync()
    {
        var news = await queryService.GetAllNewsWearableAsync();
        return Result<IEnumerable<NewsWearableDto>>.Success(news);
    }

    public async Task<Result<NewsWearableDto>> GetByIdWearableAsync(int id)
    {
        var news = await queryService.GetNewsByIdWearableAsync(id);
        if (news is null)
            return Result<NewsWearableDto>.NotFound($"News with id {id} not found.");
        return Result<NewsWearableDto>.Success(news);
    }

    public async Task<Result<NewsDto>> CreateAsync(int authorId, CreateNewsDto dto)
    {
        var news = new News
        {
            Title = dto.Title,
            AuthorId = authorId,
            Content = dto.Content,
            ImageUrl = dto.ImageUrl,
            PublishedAt = DateTime.UtcNow
        };

        await newsRepository.AddAsync(news);

        var result = await queryService.GetNewsByIdAsync(news.Id);
        return Result<NewsDto>.Success(result!, 201);
    }

    public async Task<Result<NewsDto>> UpdateAsync(int id, UpdateNewsDto dto)
    {
        var news = await newsRepository.GetByIdAsync(id);
        if (news is null)
            return Result<NewsDto>.NotFound($"News with id {id} not found.");

        if (dto.Title is not null) news.Title = dto.Title;
        if (dto.Content is not null) news.Content = dto.Content;
        if (dto.ImageUrl is not null) news.ImageUrl = dto.ImageUrl;

        await newsRepository.UpdateAsync(news);

        var result = await queryService.GetNewsByIdAsync(news.Id);
        return Result<NewsDto>.Success(result!);
    }

    public async Task<Result<bool>> DeleteAsync(int id)
    {
        var news = await newsRepository.GetByIdAsync(id);
        if (news is null)
            return Result<bool>.NotFound($"News with id {id} not found.");

        await newsRepository.DeleteAsync(news);
        return Result<bool>.Success(true);
    }
}
