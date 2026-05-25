using PWA_API.Application.DTOs.Favorites;
using PWA_API.Application.Interfaces.Repositories;
using PWA_API.Application.Interfaces.Services;
using PWA_API.Domain.Common;
using PWA_API.Domain.Entities;

namespace PWA_API.Application.Services;

public class FavoriteService(
    IFavoriteRepository favoriteRepository,
    INewsRepository newsRepository,
    INewsQueryService queryService) : IFavoriteService
{
    public async Task<Result<IEnumerable<FavoriteDto>>> GetByUserAsync(int userId)
    {
        var favorites = await queryService.GetFavoritesByUserAsync(userId);
        return Result<IEnumerable<FavoriteDto>>.Success(favorites);
    }

    public async Task<Result<FavoriteDto>> AddAsync(int userId, int newsId)
    {
        var news = await newsRepository.GetByIdAsync(newsId);
        if (news is null)
            return Result<FavoriteDto>.NotFound($"News with id {newsId} not found.");

        var existing = await favoriteRepository.GetByUserAndNewsAsync(userId, newsId);
        if (existing is not null)
            return Result<FavoriteDto>.Failure("News is already in favorites.");

        var favorite = new Favorite { UserId = userId, NewsId = newsId };
        await favoriteRepository.AddAsync(favorite);

        var result = new FavoriteDto
        {
            Id = favorite.Id,
            NewsId = newsId,
            NewsTitle = news.Title,
            NewsImageUrl = news.ImageUrl,
            AddedAt = favorite.AddedAt
        };
        return Result<FavoriteDto>.Success(result, 201);
    }

    public async Task<Result<bool>> RemoveAsync(int userId, int newsId)
    {
        var favorite = await favoriteRepository.GetByUserAndNewsAsync(userId, newsId);
        if (favorite is null)
            return Result<bool>.NotFound("Favorite not found.");

        await favoriteRepository.DeleteAsync(favorite);
        return Result<bool>.Success(true);
    }
}
