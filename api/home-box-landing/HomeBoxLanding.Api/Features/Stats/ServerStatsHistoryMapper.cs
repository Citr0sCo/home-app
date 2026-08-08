using HomeBoxLanding.Api.Features.Stats.Types;

namespace HomeBoxLanding.Api.Features.Stats;

public static class ServerStatsHistoryMapper
{
    public static ServerStatsHistoryRecord Map(StatsResponse response, DateTime recordedAt)
    {
        var stats = response.Stats;
        var memoryUsed = stats.Sum(stat => stat.MemoryUsage?.Used ?? 0);
        var memoryTotal = stats.Max(stat => stat.MemoryUsage?.Total ?? 0);
        var diskUsed = stats.Max(stat => stat.DiskUsage?.Used ?? 0);
        var diskTotal = stats.Max(stat => stat.DiskUsage?.Total ?? 0);

        return new ServerStatsHistoryRecord
        {
            Identifier = Guid.NewGuid(),
            RecordedAt = recordedAt,
            CpuPercentage = stats.Sum(stat => stat.CpuUsage?.Percentage ?? 0),
            MemoryPercentage = memoryTotal > 0 ? memoryUsed / memoryTotal * 100 : 0,
            MemoryUsed = memoryUsed,
            MemoryTotal = memoryTotal,
            DiskPercentage = diskTotal > 0 ? diskUsed / diskTotal * 100 : 0,
            DiskUsed = diskUsed,
            DiskTotal = diskTotal
        };
    }
}
