using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PWA_API.Domain.Entities;

namespace PWA_API.Infrastructure.Persistence.Configurations;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.TraceId).IsRequired().HasMaxLength(64);
        builder.Property(a => a.Username).HasMaxLength(50);
        builder.Property(a => a.Role).HasMaxLength(20);
        builder.Property(a => a.Method).IsRequired().HasMaxLength(10);
        builder.Property(a => a.Path).IsRequired().HasMaxLength(300);
        builder.Property(a => a.Action).IsRequired().HasMaxLength(120);
        builder.Property(a => a.IpAddress).HasMaxLength(64);
        builder.Property(a => a.Error).HasMaxLength(500);

        // The audit view is always "most recent first", optionally filtered to
        // failures only — both are covered by these indexes.
        builder.HasIndex(a => a.OccurredAt).IsDescending();
        builder.HasIndex(a => a.Success);
        builder.HasIndex(a => a.TraceId);
    }
}
