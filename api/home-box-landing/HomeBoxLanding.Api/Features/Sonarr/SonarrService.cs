using HomeBoxLanding.Api.Features.Settings;
using HomeBoxLanding.Api.Features.CustomLinkWidgets;
using HomeBoxLanding.Api.Core.Events.Types;
using HomeBoxLanding.Api.Features.Links;
using HomeBoxLanding.Api.Features.Links.Types;
using HomeBoxLanding.Api.Features.Sonarr.Types;
using HomeBoxLanding.Api.Features.WebSockets.Types;
using Newtonsoft.Json;

namespace HomeBoxLanding.Api.Features.Sonarr;

public class SonarrService : ISubscriber
{
    private readonly LinksService _linksService;
    private readonly WidgetCacheService _widgetCache;
    private bool _isStarted = false;

    public SonarrService(LinksService linksService, WidgetCacheService? widgetCache = null)
    {
        _linksService = linksService;
        _widgetCache = widgetCache ?? new WidgetCacheService();
    }

    public SonarrActivityResponse GetActivity()
    {
        var link = _linksService.GetAllLinks().Links.FirstOrDefault(x => x.Name?.Contains("SONARR", StringComparison.OrdinalIgnoreCase) == true);
        return link?.Identifier is Guid linkIdentifier
            ? _widgetCache.Get<SonarrActivityResponse>(linkIdentifier, WidgetTypes.Sonarr) ?? new SonarrActivityResponse()
            : new SonarrActivityResponse();
    }

    public SonarrActivityResponse RefreshActivity()
    {
        var link = _linksService.GetAllLinks().Links.FirstOrDefault(x => x.Name?.Contains("SONARR", StringComparison.OrdinalIgnoreCase) == true);
        if (link?.Identifier is not Guid linkIdentifier)
            return new SonarrActivityResponse();

        var totalSeries = GetTotalSeries(link);
        var totalMissing = GetTotalMissing(link);
        var totalQueue = GetTotalQueue(link);
        var health = GetHealth(link);
        if (totalSeries == null)
            return new SonarrActivityResponse();

        var activity = new SonarrActivityResponse
        {
            TotalNumberOfSeries = totalSeries.Count,
            TotalNumberOfQueuedEpisodes = totalQueue.Total,
            TotalNumberOfMissingEpisodes = totalMissing.Total,
            Health = health
        };
        _widgetCache.Save(linkIdentifier, WidgetTypes.Sonarr, activity);
        return activity;
    }

    private List<SonarrSeries> GetTotalSeries(Link link)
    {
        var httpClient = new HttpClient();
        httpClient.Timeout = TimeSpan.FromSeconds(20);
        var result = httpClient.GetAsync($"{link.Url}api/v3/series?apiKey={SettingsService.ResolveValue("ASPNETCORE_SONARR_API_KEY")}").Result;
        var response = result.Content.ReadAsStringAsync().Result;

        List<SonarrSeries>? parsedResponse;

        try
        {
            parsedResponse = JsonConvert.DeserializeObject<List<SonarrSeries>>(response);
        }
        catch (Exception)
        {
            return new List<SonarrSeries>();
        }

        return parsedResponse ?? new List<SonarrSeries>();
    }

    private SonarrMissing GetTotalMissing(Link link)
    {
        var httpClient = new HttpClient();
        httpClient.Timeout = TimeSpan.FromSeconds(20);
        var result = httpClient.GetAsync($"{link.Url}api/v3/wanted/missing?apiKey={SettingsService.ResolveValue("ASPNETCORE_SONARR_API_KEY")}").Result;
        var response = result.Content.ReadAsStringAsync().Result;

        SonarrMissing? parsedResponse;

        try
        {
            parsedResponse = JsonConvert.DeserializeObject<SonarrMissing>(response);
        }
        catch (Exception)
        {
            return new SonarrMissing();
        }

        return parsedResponse ?? new SonarrMissing();
    }

    private SonarrQueue GetTotalQueue(Link link)
    {
        var httpClient = new HttpClient();
        httpClient.Timeout = TimeSpan.FromSeconds(20);
        var result = httpClient.GetAsync($"{link.Url}api/v3/queue?apiKey={SettingsService.ResolveValue("ASPNETCORE_SONARR_API_KEY")}").Result;
        var response = result.Content.ReadAsStringAsync().Result;

        SonarrQueue? parsedResponse;

        try
        {
            parsedResponse = JsonConvert.DeserializeObject<SonarrQueue>(response);
        }
        catch (Exception)
        {
            return new SonarrQueue();
        }

        return parsedResponse ?? new SonarrQueue();
    }

    private List<SonarrHealth> GetHealth(Link link)
    {
        var httpClient = new HttpClient();
        httpClient.Timeout = TimeSpan.FromSeconds(20);
        var result = httpClient.GetAsync($"{link.Url}api/v3/health?apiKey={SettingsService.ResolveValue("ASPNETCORE_SONARR_API_KEY")}").Result;
        var response = result.Content.ReadAsStringAsync().Result;

        List<SonarrHealth>? parsedResponse;

        try
        {
            parsedResponse = JsonConvert.DeserializeObject<List<SonarrHealth>>(response);
        }
        catch (Exception)
        {
            return new List<SonarrHealth>();
        }

        return parsedResponse ?? new List<SonarrHealth>();
    }

    public void OnStarted()
    {
        _isStarted = true;

        Task.Run(() =>
        {
            while (_isStarted)
            {
                var activity = RefreshActivity();

                WebSockets.WebSocketManager.Instance().SendToAllClients(WebSocketKey.SonarrActivity, new
                {
                    Response = new
                    {
                        Data = new
                        {
                            TotalNumberOfSeries = activity.TotalNumberOfSeries,
                            TotalNumberOfQueuedEpisodes = activity.TotalNumberOfQueuedEpisodes,
                            TotalNumberOfMissingEpisodes = activity.TotalNumberOfMissingEpisodes,
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
