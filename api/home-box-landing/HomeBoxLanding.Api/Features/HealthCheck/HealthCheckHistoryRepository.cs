using HomeBoxLanding.Api.Data;
using HomeBoxLanding.Api.Features.HealthCheck.Types;
using Microsoft.EntityFrameworkCore;

namespace HomeBoxLanding.Api.Features.HealthCheck;

public interface IHealthCheckHistoryRepository
{
    Task SaveAsync(HealthCheckHistoryRecord record, CancellationToken cancellationToken = default);
    Task<List<HealthCheckHistoryRecord>> GetSinceAsync(DateTime since, CancellationToken cancellationToken = default);
    Task<int> DeleteOlderThanAsync(DateTime cutoff, CancellationToken cancellationToken = default);
}

public class HealthCheckHistoryRepository : IHealthCheckHistoryRepository
{
    public async Task SaveAsync(HealthCheckHistoryRecord record, CancellationToken cancellationToken = default)
    {
        await using var context = new DatabaseContext();
        context.HealthCheckHistory.Add(record);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<List<HealthCheckHistoryRecord>> GetSinceAsync(
        DateTime since,
        CancellationToken cancellationToken = default)
    {
        await using var context = new DatabaseContext();
        return await context.HealthCheckHistory
            .AsNoTracking()
            .Where(record => record.RecordedAt >= since)
            .OrderBy(record => record.RecordedAt)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<int> DeleteOlderThanAsync(
        DateTime cutoff,
        CancellationToken cancellationToken = default)
    {
        await using var context = new DatabaseContext();
        return await context.HealthCheckHistory
            .Where(record => record.RecordedAt < cutoff)
            .ExecuteDeleteAsync(cancellationToken)
            .ConfigureAwait(false);
    }
}
