namespace PWA_API.Application.DTOs.Favorites;

public class FavoriteDto
{
    public int Id { get; set; }
    public int NewsId { get; set; }
    public string NewsTitle { get; set; } = string.Empty;
    public string? NewsImageUrl { get; set; }
    public DateTime AddedAt { get; set; }
}
