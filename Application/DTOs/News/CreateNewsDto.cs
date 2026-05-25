namespace PWA_API.Application.DTOs.News;

public record CreateNewsDto(
    string Title,
    string Content,
    string? ImageUrl
);
