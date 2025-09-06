using HomeBoxLanding.Api.Core.Events.Types;
using HomeBoxLanding.Api.Features.Lidarr.Types;
using HomeBoxLanding.Api.Features.Links;
using HomeBoxLanding.Api.Features.Links.Types;
using HomeBoxLanding.Api.Features.Readarr.Types;
using HomeBoxLanding.Api.Features.WebSockets.Types;
using Newtonsoft.Json;

namespace HomeBoxLanding.Api.Features.Readarr;

public class ReadarrService : ISubscriber
{
    private readonly LinksService _linksService;
    private bool _isStarted = false;
    private string API_KEY;

    public ReadarrService(LinksService linksService)
    {
        _linksService = linksService;
        API_KEY = Environment.GetEnvironmentVariable("ASPNETCORE_READARR_API_KEY");
    }

    public ReadarrActivityResponse GetActivity()
    {
        var link = _linksService.GetAllLinks().Links.FirstOrDefault(x => x.Name.ToUpper().Contains("READARR"));

        if (link == null)
        {
            return new ReadarrActivityResponse();
        }

        var totalBooks = GetTotalBooks(link);
        
        var totalQueue = GetTotalQueue(link);
        
        var health = GetHealth(link);

        if (totalBooks == null)
        {
            return new ReadarrActivityResponse();
        }

        return new ReadarrActivityResponse
        {
            TotalNumberOfBooks = totalBooks.Sum(x => x.Statistics.BookCount),
            TotalNumberOfQueuedBooks = totalQueue.Total,
            TotalMissingBooks = totalBooks.Sum(x => x.Statistics.TotalBookCount - x.Statistics.BookCount),
            Health = health
        };
    }

    private List<ReadarrTrack> GetTotalBooks(Link link)
    {
        var httpClient = new HttpClient();
        httpClient.Timeout = TimeSpan.FromSeconds(20);
        var result = httpClient.GetAsync($"{link.Url}api/v1/author?apiKey={API_KEY}").Result;
        var response = result.Content.ReadAsStringAsync().Result;

        List<ReadarrTrack>? parsedResponse;
        
        try
        {
            parsedResponse = JsonConvert.DeserializeObject<List<ReadarrTrack>>(response);
        }
        catch (Exception)
        {
            return new List<ReadarrTrack>();
        }

        return parsedResponse ?? new List<ReadarrTrack>();
    }

    private ReadarrQueue GetTotalQueue(Link link)
    {
        var httpClient = new HttpClient();
        httpClient.Timeout = TimeSpan.FromSeconds(20);
        var result = httpClient.GetAsync($"{link.Url}api/v1/queue?apiKey={API_KEY}").Result;
        var response = result.Content.ReadAsStringAsync().Result;

        ReadarrQueue? parsedResponse;
        
        try
        {
            parsedResponse = JsonConvert.DeserializeObject<ReadarrQueue>(response);
        }
        catch (Exception)
        {
            return new ReadarrQueue();
        }

        return parsedResponse ?? new ReadarrQueue();
    }

    private List<ReadarrHealth> GetHealth(Link link)
    {
        var httpClient = new HttpClient();
        httpClient.Timeout = TimeSpan.FromSeconds(20);
        var result = httpClient.GetAsync($"{link.Url}api/v1/health?apiKey={API_KEY}").Result;
        var response = result.Content.ReadAsStringAsync().Result;

        List<ReadarrHealth>? parsedResponse;
        
        try
        {
            parsedResponse = JsonConvert.DeserializeObject<List<ReadarrHealth>>(response);
        }
        catch (Exception)
        {
            return new List<ReadarrHealth>();
        }

        return parsedResponse ?? new List<ReadarrHealth>();
    }

    public void OnStarted()
    {
        _isStarted = true;

        Task.Run(() =>
        {
            while (_isStarted)
            {
                var activity = GetActivity();    
                    
                WebSockets.WebSocketManager.Instance().SendToAllClients(WebSocketKey.ReadarrActivity, new
                {
                    Response = new
                    {
                        Data = new
                        {
                            TotalNumberOfBooks = activity.TotalNumberOfBooks,
                            TotalNumberOfQueuedBooks = activity.TotalNumberOfQueuedBooks,
                            TotalMissingBooks = activity.TotalMissingBooks,
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