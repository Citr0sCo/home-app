using HomeBoxLanding.Api.Core.Events.Types;
using HomeBoxLanding.Api.Features.Links;

namespace HomeBoxLanding.Api.Features.HealthCheck;

public class HealthCheckBackgroundService : ISubscriber
{
    private static readonly TimeSpan CheckInterval = TimeSpan.FromMinutes(15);

    private readonly ILinksRepository _linksRepository;
    private readonly HealthCheckService _healthCheckService;
    private CancellationTokenSource? _lifetimeCancellation;
    private Task? _healthCheckTask;

    public HealthCheckBackgroundService(
        IHttpClientFactory httpClientFactory,
        ILinksRepository? linksRepository = null,
        IHealthCheckHistoryRepository? historyRepository = null)
    {
        _linksRepository = linksRepository ?? new LinksRepository();
        _healthCheckService = new HealthCheckService(
            httpClientFactory,
            historyRepository ?? new HealthCheckHistoryRepository());
    }

    public void OnStarted()
    {
        if (_healthCheckTask is { IsCompleted: false })
            return;

        var cancellation = new CancellationTokenSource();
        _lifetimeCancellation = cancellation;
        _healthCheckTask = Task.Run(() => RunHealthCheckLoopAsync(cancellation.Token));
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

    private async Task RunHealthCheckLoopAsync(CancellationToken cancellationToken)
    {
        using var timer = new PeriodicTimer(CheckInterval);

        while (!cancellationToken.IsCancellationRequested)
        {
            await CheckAllLinksAsync(cancellationToken).ConfigureAwait(false);

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

    private async Task CheckAllLinksAsync(CancellationToken cancellationToken)
    {
        foreach (var link in _linksRepository.GetAll())
        {
            if (cancellationToken.IsCancellationRequested)
                return;

            if (link.Identifier == Guid.Empty)
                continue;

            var target = link.Host;
            if (!string.IsNullOrWhiteSpace(target) && link.Port > 0)
                target = $"{target}:{link.Port}";
            else if (string.IsNullOrWhiteSpace(target))
                target = link.Url ?? string.Empty;

            try
            {
                await _healthCheckService
                    .PerformHealthCheck(target, link.IsSecure, link.Identifier, cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                return;
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Failed to check health of link {link.Identifier}: {exception.Message}");
            }
        }
    }
}
