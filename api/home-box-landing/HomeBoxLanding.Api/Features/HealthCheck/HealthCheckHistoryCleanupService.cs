using HomeBoxLanding.Api.Core.Events.Types;

namespace HomeBoxLanding.Api.Features.HealthCheck;

public class HealthCheckHistoryCleanupService : ISubscriber
{
    private readonly IHealthCheckHistoryRepository _historyRepository;
    private CancellationTokenSource? _lifetimeCancellation;
    private Task? _cleanupTask;

    public HealthCheckHistoryCleanupService(IHealthCheckHistoryRepository? historyRepository = null)
    {
        _historyRepository = historyRepository ?? new HealthCheckHistoryRepository();
    }

    public void OnStarted()
    {
        if (_cleanupTask is { IsCompleted: false })
            return;

        var cancellation = new CancellationTokenSource();
        _lifetimeCancellation = cancellation;
        _cleanupTask = Task.Run(() => RunCleanupLoopAsync(cancellation.Token));
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

    private async Task RunCleanupLoopAsync(CancellationToken cancellationToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromHours(1));

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await _historyRepository.DeleteOlderThanAsync(
                    DateTime.UtcNow.AddDays(-7),
                    cancellationToken).ConfigureAwait(false);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                return;
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Failed to clean up health check history: {exception.Message}");
            }

            try
            {
                if (!await timer.WaitForNextTickAsync(cancellationToken).ConfigureAwait(false))
                    return;
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                return;
            }
        }
    }
}
