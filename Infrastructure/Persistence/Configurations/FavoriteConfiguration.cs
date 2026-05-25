using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PWA_API.Domain.Entities;

namespace PWA_API.Infrastructure.Persistence.Configurations;

public class FavoriteConfiguration : IEntityTypeConfiguration<Favorite>
{
    public void Configure(EntityTypeBuilder<Favorite> builder)
    {
        builder.HasKey(f => f.Id);
        builder.HasIndex(f => new { f.UserId, f.NewsId }).IsUnique();

        builder.HasOne(f => f.User)
            .WithMany(u => u.Favorites)
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(f => f.News)
            .WithMany(n => n.Favorites)
            .HasForeignKey(f => f.NewsId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
