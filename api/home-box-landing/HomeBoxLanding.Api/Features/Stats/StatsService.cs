using HomeBoxLanding.Api.Core.Events.Types;
using HomeBoxLanding.Api.Core.Shell;
using HomeBoxLanding.Api.Core.Types;
using HomeBoxLanding.Api.Features.Stats.Types;
using HomeBoxLanding.Api.Features.WebSockets.Types;
using Newtonsoft.Json;
using WebSocketManager = HomeBoxLanding.Api.Features.WebSockets.WebSocketManager;

namespace HomeBoxLanding.Api.Features.Stats;

public class StatsService : ISubscriber
{
    private readonly IStatsServiceCache _cacheService;
    private readonly IShellService _shellService;
    private readonly IServerStatsHistoryRepository _historyRepository;
    private int? _cpuCount;
    private CancellationTokenSource? _lifetimeCancellation;
    private Task? _statsTask;

    public StatsService(
        IShellService shellService,
        IStatsServiceCache cacheService,
        IServerStatsHistoryRepository? historyRepository = null)
    {
        _shellService = shellService;
        _cacheService = cacheService;
        _historyRepository = historyRepository ?? new ServerStatsHistoryRepository();
    }

    public void OnStarted()
    {
        if (_statsTask is { IsCompleted: false })
            return;

        var cancellation = new CancellationTokenSource();
        _lifetimeCancellation = cancellation;
        _statsTask = Task.Run(() => RunStatsLoopAsync(cancellation.Token));
    }

    public void OnStopping()
    {
        _lifetimeCancellation?.Cancel();
    }

    public void OnStopped()
    {
        _lifetimeCancellation?.Dispose();
        _lifetimeCancellation = null;
    }

    private async Task RunStatsLoopAsync(CancellationToken cancellationToken)
    {
        var nextCleanup = DateTime.UtcNow;

        while (!cancellationToken.IsCancellationRequested)
        {
            var now = DateTime.UtcNow;

            if (now >= nextCleanup)
            {
                try
                {
                    await _historyRepository.DeleteOlderThanAsync(
                        now.AddDays(-7),
                        cancellationToken).ConfigureAwait(false);
                    nextCleanup = now.AddHours(1);
                }
                catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
                {
                    return;
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"Failed to clean up server stats history: {exception.Message}");
                    nextCleanup = now.AddMinutes(5);
                }
            }

            var stats = GetServerStats(true);

            if (!stats.HasError && stats.Stats.Count > 0)
            {
                try
                {
                    await _historyRepository.SaveAsync(
                        ServerStatsHistoryMapper.Map(stats, now),
                        cancellationToken).ConfigureAwait(false);
                }
                catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
                {
                    return;
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"Failed to persist server stats: {exception.Message}");
                }
            }

            try
            {
                WebSocketManager.Instance().SendToAllClients(WebSocketKey.ServerStats, stats);
                await Task.Delay(TimeSpan.FromSeconds(15), cancellationToken).ConfigureAwait(false);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                return;
            }
        }
    }

    public async Task<ServerStatsHistoryResponse> GetServerStatsHistoryAsync(
        CancellationToken cancellationToken = default)
    {
        var to = DateTime.UtcNow;
        var from = to.AddHours(-24);

        try
        {
            var records = await _historyRepository
                .GetSinceAsync(from, cancellationToken)
                .ConfigureAwait(false);

            return new ServerStatsHistoryResponse
            {
                From = from,
                To = to,
                Samples = records.Select(record => new ServerStatsHistoryPoint
                {
                    RecordedAt = record.RecordedAt,
                    CpuPercentage = Math.Clamp(record.CpuPercentage, 0, 100),
                    MemoryPercentage = record.MemoryPercentage,
                    MemoryUsed = record.MemoryUsed,
                    MemoryTotal = record.MemoryTotal,
                    DiskPercentage = record.DiskPercentage,
                    DiskUsed = record.DiskUsed,
                    DiskTotal = record.DiskTotal
                }).ToList()
            };
        }
        catch (Exception exception)
        {
            return new ServerStatsHistoryResponse
            {
                From = from,
                To = to,
                HasError = true,
                Error = new Error
                {
                    Code = ErrorCode.FailedToGetStats,
                    UserMessage = "Failed to load server statistics history.",
                    TechnicalMessage = exception.Message
                }
            };
        }
    }

    public StatsResponse GetServerStats(bool forceCheck = false)
    {
        if (_cacheService.GetStats() != null && !forceCheck)
            return _cacheService.GetStats() ?? new StatsResponse();

        var response = new StatsResponse();

        var output = string.Empty;

        try
        {
            output = _shellService.RunOnHost("docker stats --no-stream");
        }
        catch (Exception)
        {
            return new StatsResponse
            {
                HasError = true,
                Error = new Error
                {
                    Code = ErrorCode.FailedToGetStats,
                    UserMessage = "Failed to run shell command.",
                    TechnicalMessage = $"Received the following: {output}"
                }
            };
        }

        var cpuCount = GetCpuCount();
        var lines = output.Split("\n");

        if (lines.Length < 2)
            return new StatsResponse
            {
                HasError = true,
                Error = new Error
                {
                    Code = ErrorCode.FailedToGetStats,
                    UserMessage = "Incorrect number of lines received from shell",
                    TechnicalMessage = $"Received the following: {JsonConvert.SerializeObject(lines)}"
                }
            };

        foreach (var line in lines)
        {
            var stats = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            if (stats.Length == 0)
                continue;
            
            if(stats[0] == "CONTAINER")
                continue;
            
            if(stats.Any((x) => x == "Executing command"))
                continue;
            
            var driveInfo = new DriveInfo(AppContext.BaseDirectory);
            double totalDriveSize = driveInfo.TotalSize;
            double usedDriveSize = driveInfo.TotalSize - driveInfo.AvailableFreeSpace;
            
            response.Stats.Add(new StatModel
            {
                Name = stats[1],
                CpuUsage = new Stat
                {
                    Percentage = ParseSize(stats[2]) / cpuCount
                },
                MemoryUsage = new Stat
                {
                    Total = ParseSize(stats[5]),
                    Used = ParseSize(stats[3]),
                    Percentage = ParseSize(stats[6])
                },
                DiskUsage = new Stat
                {
                    Percentage = Math.Round(usedDriveSize / totalDriveSize, 2) * 100,
                    Total = driveInfo.TotalSize,
                    Used = driveInfo.TotalSize - driveInfo.AvailableFreeSpace
                }
            });
        }
            
        _cacheService.SetStats(response);

        return response;
    }

    private int GetCpuCount()
    {
        if (_cpuCount.HasValue)
            return _cpuCount.Value;

        try
        {
            var output = _shellService.RunOnHost("docker info --format '{{.NCPU}}'");
            if (int.TryParse(output.Trim(), out var cpuCount) && cpuCount > 0)
            {
                _cpuCount = cpuCount;
                return cpuCount;
            }
        }
        catch (Exception)
        {
            // Fall back to the process-visible CPU count when Docker info is unavailable.
        }

        _cpuCount = Math.Max(Environment.ProcessorCount, 1);
        return _cpuCount.Value;
    }

    private static double ParseSize(string toRemove)
    {
        if (toRemove.Contains("%"))
            return Math.Round(double.Parse(toRemove.Replace("%", "")), 2, MidpointRounding.ToZero);

        if (toRemove.Contains("GiB"))
            return Math.Round(double.Parse(toRemove.Replace("GiB", "")) * 1024 * 1048576d, 2,
                MidpointRounding.ToZero);

        if (toRemove.Contains("MiB"))
            return Math.Round(double.Parse(toRemove.Replace("MiB", "")) * 1048576d, 2, MidpointRounding.ToZero);

        return double.Parse(toRemove);
    }
}