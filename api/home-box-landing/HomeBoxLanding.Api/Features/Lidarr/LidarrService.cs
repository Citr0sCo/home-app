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
        var link = _linksService.GetAllLinks().Links.FirstOrDefault(x => x.Name.ToUpper().Contains("RADARR"));

        if (link == null)
        {
            return new LidarrActivityResponse();
        }

        var totalMovies = GetTotalMovies(link);
        
        var totalQueue = GetTotalQueue(link);
        
        var health = GetHealth(link);

        if (totalMovies == null)
        {
            return new LidarrActivityResponse();
        }

        return new LidarrActivityResponse
        {
            TotalNumberOfMovies = totalMovies.Count,
            TotalNumberOfQueuedMovies = totalQueue.Total,
            TotalMissingMovies = totalMovies.Count(x => x.SizeOnDisk == 0),
            Health = health
        };
    }

    private List<LidarrMovie> GetTotalMovies(Link link)
    {
        var httpClient = new HttpClient();
        httpClient.Timeout = TimeSpan.FromSeconds(20);
        var result = httpClient.GetAsync($"{link.Url}api/v3/movie?apiKey={API_KEY}").Result;
        var response = result.Content.ReadAsStringAsync().Result;

        List<LidarrMovie>? parsedResponse;
        
        try
        {
            parsedResponse = JsonConvert.DeserializeObject<List<LidarrMovie>>(response);
        }
        catch (Exception)
        {
            return new List<LidarrMovie>();
        }

        return parsedResponse ?? new List<LidarrMovie>();
    }

    private LidarrQueue GetTotalQueue(Link link)
    {
        var httpClient = new HttpClient();
        httpClient.Timeout = TimeSpan.FromSeconds(20);
        var result = httpClient.GetAsync($"{link.Url}api/v3/queue?apiKey={API_KEY}").Result;
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
        var result = httpClient.GetAsync($"{link.Url}api/v3/health?apiKey={API_KEY}").Result;
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
                            TotalNumberOfMovies = activity.TotalNumberOfMovies,
                            TotalNumberOfQueuedMovies = activity.TotalNumberOfQueuedMovies,
                            TotalMissingMovies = activity.TotalMissingMovies,
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