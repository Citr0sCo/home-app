using HomeBoxLanding.Api.Features.Stats;
using HomeBoxLanding.Api.Features.Stats.Types;
using NUnit.Framework;

namespace HomeBoxLanding.Api.Tests.Features.Stats;

public class ServerStatsHistoryMapperTests
{
    [Test]
    public void MapAggregatesContainerStatsIntoAHistorySample()
    {
        var recordedAt = new DateTime(2026, 8, 1, 12, 0, 0, DateTimeKind.Utc);
        var response = new StatsResponse
        {
            Stats = new List<StatModel>
            {
                new()
                {
                    Name = "app",
                    CpuUsage = new Stat { Percentage = 12.5 },
                    MemoryUsage = new Stat { Used = 200, Total = 1000 },
                    DiskUsage = new Stat { Used = 400, Total = 2000 }
                },
                new()
                {
                    Name = "database",
                    CpuUsage = new Stat { Percentage = 3.5 },
                    MemoryUsage = new Stat { Used = 300, Total = 1000 },
                    DiskUsage = new Stat { Used = 500, Total = 2000 }
                }
            }
        };

        var result = ServerStatsHistoryMapper.Map(response, recordedAt);

        Assert.That(result.RecordedAt, Is.EqualTo(recordedAt));
        Assert.That(result.CpuPercentage, Is.EqualTo(16));
        Assert.That(result.MemoryUsed, Is.EqualTo(500));
        Assert.That(result.MemoryTotal, Is.EqualTo(1000));
        Assert.That(result.MemoryPercentage, Is.EqualTo(50));
        Assert.That(result.DiskUsed, Is.EqualTo(500));
        Assert.That(result.DiskTotal, Is.EqualTo(2000));
        Assert.That(result.DiskPercentage, Is.EqualTo(25));
    }
}
