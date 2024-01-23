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
        _buildService = new BuildsService(new BuildsRepository(), ShellService.Instance(), new DockerBuildsRepository());
    }

    public static ISubscriber Instance()
    {
        if (_instance == null)
            _instance = new DockerAutoUpdate();

        return _instance;
    }

    private async Task StartPolling()
    {
        var lastRunAt = DateTime.MinValue;
        
        while (_isPolling)
        {
            Console.WriteLine("Checking if should update docker apps...");
            
            var currentTime = DateTime.Now;
            var updateWindowStart = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 3, 0, 0);
            var updateWindowEnd = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 3, 15, 0);
            
            if (currentTime < updateWindowStart || currentTime > updateWindowEnd) // if outside update window
            {
                Console.WriteLine("Outside of update window, skipping...");
                continue;
            }
            
            if(currentTime.AddDays(-1) > lastRunAt) // if already ran today
            {
                Console.WriteLine("Already updated today, skipping...");
                continue;
            }
            
            Console.WriteLine("Updating docker apps...");
            
            Console.WriteLine("UPDATE DISABLED");
            //_buildService.UpdateAllDockerApps();
            lastRunAt = DateTime.Now;
            
            Console.WriteLine("Finished updating docker apps, waiting for 24 hours...");
            Thread.Sleep(1000 * 60); // 1 Minute
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