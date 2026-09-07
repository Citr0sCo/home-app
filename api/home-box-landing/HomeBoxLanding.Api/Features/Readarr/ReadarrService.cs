using HomeBoxLanding.Api.Features.Settings;
using HomeBoxLanding.Api.Features.CustomLinkWidgets;
using HomeBoxLanding.Api.Core.Events.Types;
using HomeBoxLanding.Api.Features.Links;
using HomeBoxLanding.Api.Features.Links.Types;
using HomeBoxLanding.Api.Features.Readarr.Types;
using HomeBoxLanding.Api.Features.WebSockets.Types;
using Newtonsoft.Json;

namespace HomeBoxLanding.Api.Features.Readarr;

public class ReadarrService : ISubscriber
{
    private readonly LinksService _linksService;
    private readonly WidgetCacheService _widgetCache;
    private bool _isStarted = false;

    public ReadarrService(LinksService linksService, WidgetCacheService? widgetCache = null)
    {
        _linksService = linksService;
        _widgetCache = widgetCache ?? new WidgetCacheService();
    }

    public ReadarrActivityResponse GetActivity()
    {
        var link = _linksService.GetAllLinks().Links.FirstOrDefault(x => x.Name?.Contains("READARR", StringComparison.OrdinalIgnoreCase) == true);
        return link?.Identifier is Guid linkIdentifier
            ? _widgetCache.Get<ReadarrActivityResponse>(linkIdentifier, WidgetTypes.Readarr) ?? new ReadarrActivityResponse()
            : new ReadarrActivityResponse();
    }

    public ReadarrActivityResponse RefreshActivity()
    {
        var link = _linksService.GetAllLinks().Links.FirstOrDefault(x => x.Name?.Contains("READARR", StringComparison.OrdinalIgnoreCase) == true);
        if (link?.Identifier is not Guid linkIdentifier)
            return new ReadarrActivityResponse();

        var totalBooks = GetTotalBooks(link);
        var totalQueue = GetTotalQueue(link);
        var health = GetHealth(link);
        if (totalBooks == null)
            return new ReadarrActivityResponse();

        var activity = new ReadarrActivityResponse
        {
            TotalNumberOfBooks = totalBooks.Sum(x => x.Statistics.BookCount),
            TotalNumberOfQueuedBooks = totalQueue.Total,
            TotalMissingBooks = totalBooks.Sum(x => x.Statistics.BookCount - x.Statistics.AvailableBookCount),
            Health = health
        };
        _widgetCache.Save(linkIdentifier, WidgetTypes.Readarr, activity);
        return activity;
    }

    private List<ReadarrTrack> GetTotalBooks(Link link)
    {
        var httpClient = new HttpClient();
        httpClient.Timeout = TimeSpan.FromSeconds(20);
        var result = httpClient.GetAsync($"{link.Url}api/v1/author?apiKey={SettingsService.ResolveValue("ASPNETCORE_READARR_API_KEY")}").Result;
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
        var result = httpClient.GetAsync($"{link.Url}api/v1/queue?apiKey={SettingsService.ResolveValue("ASPNETCORE_READARR_API_KEY")}").Result;
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
        var result = httpClient.GetAsync($"{link.Url}api/v1/health?apiKey={SettingsService.ResolveValue("ASPNETCORE_READARR_API_KEY")}").Result;
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
                var activity = RefreshActivity();

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

                Thread.Sleep(TimeSpan.FromMinutes(15));
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
