using HomeBoxLanding.Api.Core.Shell;
using HomeBoxLanding.Api.Features.Stats;
using HomeBoxLanding.Api.Features.Stats.Types;
using Moq;
using NUnit.Framework;

namespace HomeBoxLanding.Api.Tests.Features.Stats;

public class ServerStatsHistoryEndpointTests
{
    [Test]
    public async Task ReturnsCurrentSampleWhenHistoryHasNotCollectedARecordYet()
    {
        var shellService = new Mock<IShellService>();
        shellService
            .Setup(service => service.RunOnHost("docker stats --no-stream"))
            .Returns("CONTAINER ID NAME CPU MEM USAGE / LIMIT MEM % NET I/O BLOCK I/O PIDS\nabc home-app 20.00% 100MiB / 1GiB 10.00% 0B / 0B 0B / 0B 1");

        var service = new StatsService(
            shellService.Object,
            new EmptyStatsCache(),
            new EmptyHistoryRepository(),
            cpuCount: 1);

        var result = await service.GetServerStatsHistoryAsync();

        Assert.That(result.HasError, Is.False);
        Assert.That(result.Samples, Has.Count.EqualTo(1));
        Assert.That(result.Samples[0].CpuPercentage, Is.EqualTo(20));
    }

    private sealed class EmptyStatsCache : IStatsServiceCache
    {
        public StatsResponse? GetStats() => null;
        public void SetStats(StatsResponse stats) { }
    }

    private sealed class EmptyHistoryRepository : IServerStatsHistoryRepository
    {
        public Task SaveAsync(ServerStatsHistoryRecord record, CancellationToken cancellationToken = default) => Task.CompletedTask;
        public Task<List<ServerStatsHistoryRecord>> GetSinceAsync(DateTime since, CancellationToken cancellationToken = default) => Task.FromResult(new List<ServerStatsHistoryRecord>());
        public Task<int> DeleteOlderThanAsync(DateTime cutoff, CancellationToken cancellationToken = default) => Task.FromResult(0);
    }
}
