namespace PWA_API.Application.DTOs.News;

public record UpdateNewsDto(
    string? Title,
    string? Content,
    string? ImageUrl
);
