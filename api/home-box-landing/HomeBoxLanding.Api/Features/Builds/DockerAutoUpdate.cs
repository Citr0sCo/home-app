using HomeBoxLanding.Api.Core.Events.Types;
using HomeBoxLanding.Api.Core.Shell;

namespace HomeBoxLanding.Api.Features.Builds;

public class DockerAutoUpdate : ISubscriber
{
    private static DockerAutoUpdate _instance;
    private bool _isPolling = false;
    private readonly BuildsService _buildService;

    public DockerAutoUpdate()
    {
        _buildService = new BuildsService(new BuildsRepository(), ShellService.Instance());
    }

    public static ISubscriber Instance()
    {
        if (_instance == null)
            _instance = new DockerAutoUpdate();

        return _instance;
    }

    private async Task StartPolling()
    {
        while (_isPolling)
        {
            Console.WriteLine("Updating docker apps...");

            _buildService.UpdateAllDockerApps();
            
            Console.WriteLine("Finished updating docker apps, waiting for 24 hours...");
            Thread.Sleep(1000 * 60 * 60 * 24); // 24 Hours
        }
    }

    public void OnStarted()
    {
        _isPolling = true;
        StartPolling().ConfigureAwait(false);
    }

    public void OnStopping()
    {
        _isPolling = false;
    }

    public void OnStopped()
    {
        _isPolling = false;
    }
    
}