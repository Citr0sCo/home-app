using HomeBoxLanding.Api.Core.Events.Types;
using HomeBoxLanding.Api.Features.Lidarr.Types;
using HomeBoxLanding.Api.Features.Links;
using HomeBoxLanding.Api.Features.Links.Types;
using HomeBoxLanding.Api.Features.WebSockets.Types;
using Newtonsoft.Json;

namespace HomeBoxLanding.Api.Features.Lidarr;

public class LidarrService : ISubscriber
{
    private readonly LinksService _linksService;
    private bool _isStarted = false;
    private string API_KEY;

    public LidarrService(LinksService linksService)
    {
        _linksService = linksService;
        API_KEY = Environment.GetEnvironmentVariable("ASPNETCORE_LIDARR_API_KEY");
    }

    public LidarrActivityResponse GetActivity()
    {
        var link = _linksService.GetAllLinks().Links.FirstOrDefault(x => x.Name.ToUpper().Contains("LIDARR"));

        if (link == null)
        {
            return new LidarrActivityResponse();
        }

        var totalTracks = GetTotalTracks(link);
        
        var totalQueue = GetTotalQueue(link);
        
        var health = GetHealth(link);

        if (totalTracks == null)
        {
            return new LidarrActivityResponse();
        }

        return new LidarrActivityResponse
        {
            TotalNumberOfTracks = totalTracks.Sum(x => x.Statistics?.TrackFileCount ?? 0),
            TotalNumberOfQueuedTracks = totalQueue.Total,
            TotalMissingTracks = totalTracks.Sum(x => (x.Statistics?.TotalTrackCount ?? 0) - (x.Statistics?.TrackFileCount ?? 0)),
            Health = health
        };
    }

    private List<LidarrTrack> GetTotalTracks(Link link)
    {
        var httpClient = new HttpClient();
        httpClient.Timeout = TimeSpan.FromSeconds(20);
        var result = httpClient.GetAsync($"{link.Url}api/v1/artist?apiKey={API_KEY}").Result;
        var response = result.Content.ReadAsStringAsync().Result;

        List<LidarrTrack>? parsedResponse;
        
        try
        {
            parsedResponse = JsonConvert.DeserializeObject<List<LidarrTrack>>(response);
        }
        catch (Exception)
        {
            return new List<LidarrTrack>();
        }

        return parsedResponse ?? new List<LidarrTrack>();
    }

    private LidarrQueue GetTotalQueue(Link link)
    {
        var httpClient = new HttpClient();
        httpClient.Timeout = TimeSpan.FromSeconds(20);
        var result = httpClient.GetAsync($"{link.Url}api/v1/queue?apiKey={API_KEY}").Result;
        var response = result.Content.ReadAsStringAsync().Result;

        LidarrQueue? parsedResponse;
        
        try
        {
            parsedResponse = JsonConvert.DeserializeObject<LidarrQueue>(response);
        }
        catch (Exception)
        {
            return new LidarrQueue();
        }

        return parsedResponse ?? new LidarrQueue();
    }

    private List<LidarrHealth> GetHealth(Link link)
    {
        var httpClient = new HttpClient();
        httpClient.Timeout = TimeSpan.FromSeconds(20);
        var result = httpClient.GetAsync($"{link.Url}api/v1/health?apiKey={API_KEY}").Result;
        var response = result.Content.ReadAsStringAsync().Result;

        List<LidarrHealth>? parsedResponse;
        
        try
        {
            parsedResponse = JsonConvert.DeserializeObject<List<LidarrHealth>>(response);
        }
        catch (Exception)
        {
            return new List<LidarrHealth>();
        }

        return parsedResponse ?? new List<LidarrHealth>();
    }

    public void OnStarted()
    {
        _isStarted = true;

        Task.Run(() =>
        {
            while (_isStarted)
            {
                var activity = GetActivity();    
                    
                WebSockets.WebSocketManager.Instance().SendToAllClients(WebSocketKey.LidarrActivity, new
                {
                    Response = new
                    {
                        Data = new
                        {
                            TotalNumberOfTracks = activity.TotalNumberOfTracks,
                            TotalNumberOfQueuedTracks = activity.TotalNumberOfQueuedTracks,
                            TotalMissingTracks = activity.TotalMissingTracks,
                            Health = activity.Health.ConvertAll(x => new
                            {
                                Source = x.Source,
                                Type = x.Type,
                                Message = x.Message,
                                WikiUrl = x.WikiUrl
                            })
                        }
                    }
                });
                
                Thread.Sleep(5000);
            }
        }, CancellationToken.None);
    }

    public void OnStopping()
    {
        _isStarted = false;
    }

    public void OnStopped()
    {
        // Do nothing
    }
}