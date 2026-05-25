namespace PWA_API.Domain.Entities;

public class News
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int AuthorId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime PublishedAt { get; set; } = DateTime.UtcNow;
    public string? ImageUrl { get; set; }

    public User Author { get; set; } = null!;
    public ICollection<Favorite> Favorites { get; set; } = [];
}
