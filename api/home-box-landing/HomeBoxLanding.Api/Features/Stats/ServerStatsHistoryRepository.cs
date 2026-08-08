using HomeBoxLanding.Api.Data;
using HomeBoxLanding.Api.Features.Stats.Types;
using Microsoft.EntityFrameworkCore;

namespace HomeBoxLanding.Api.Features.Stats;

public interface IServerStatsHistoryRepository
{
    Task SaveAsync(ServerStatsHistoryRecord record, CancellationToken cancellationToken = default);
    Task<List<ServerStatsHistoryRecord>> GetSinceAsync(DateTime since, CancellationToken cancellationToken = default);
    Task<int> DeleteOlderThanAsync(DateTime cutoff, CancellationToken cancellationToken = default);
}

public class ServerStatsHistoryRepository : IServerStatsHistoryRepository
{
    public async Task SaveAsync(ServerStatsHistoryRecord record, CancellationToken cancellationToken = default)
    {
        await using var context = new DatabaseContext();
        context.ServerStatsHistory.Add(record);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<List<ServerStatsHistoryRecord>> GetSinceAsync(
        DateTime since,
        CancellationToken cancellationToken = default)
    {
        await using var context = new DatabaseContext();
        return await context.ServerStatsHistory
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
        return await context.ServerStatsHistory
            .Where(record => record.RecordedAt < cutoff)
            .ExecuteDeleteAsync(cancellationToken)
            .ConfigureAwait(false);
    }
}
