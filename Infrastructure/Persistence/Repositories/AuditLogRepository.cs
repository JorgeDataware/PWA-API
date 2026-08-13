using Microsoft.EntityFrameworkCore;
using PWA_API.Application.Interfaces.Repositories;
using PWA_API.Domain.Entities;

namespace PWA_API.Infrastructure.Persistence.Repositories;

public class AuditLogRepository(AppDbContext context) : IAuditLogRepository
{
    public async Task AddAsync(AuditLog entry)
    {
        context.AuditLogs.Add(entry);
        await context.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<AuditLog>> GetRecentAsync(int limit, bool onlyFailures)
    {
        var query = context.AuditLogs.AsNoTracking();
        if (onlyFailures) query = query.Where(a => !a.Success);
        return await query
            .OrderByDescending(a => a.OccurredAt)
            .ThenByDescending(a => a.Id)
            .Take(limit)
            .ToListAsync();
    }

    public Task<AuditLog?> GetByTraceIdAsync(string traceId) =>
        context.AuditLogs.AsNoTracking().FirstOrDefaultAsync(a => a.TraceId == traceId);
}
