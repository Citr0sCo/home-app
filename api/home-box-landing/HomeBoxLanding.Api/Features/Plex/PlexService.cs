using HomeBoxLanding.Api.Features.Settings;
using HomeBoxLanding.Api.Features.CustomLinkWidgets;
using HomeBoxLanding.Api.Core.Events.Types;
using HomeBoxLanding.Api.Features.Links;
using HomeBoxLanding.Api.Features.Plex.Types;
using HomeBoxLanding.Api.Features.WebSockets.Types;
using Newtonsoft.Json;

namespace HomeBoxLanding.Api.Features.Plex;

public class PlexService : ISubscriber
{
    private readonly LinksService _linksService;
    private readonly WidgetCacheService _widgetCache;
    private bool _isStarted = false;

    public PlexService(LinksService linksService, WidgetCacheService? widgetCache = null)
    {
        _linksService = linksService;
        _widgetCache = widgetCache ?? new WidgetCacheService();
    }

    public PlexActivityResponse GetActivity()
    {
        var link = _linksService.GetAllLinks().Links.FirstOrDefault(x => x.Name?.Contains("TAUTULLI", StringComparison.OrdinalIgnoreCase) == true);
        return link?.Identifier is Guid linkIdentifier
            ? _widgetCache.Get<PlexActivityResponse>(linkIdentifier, WidgetTypes.Plex) ?? new PlexActivityResponse()
            : new PlexActivityResponse();
    }

    public PlexActivityResponse RefreshActivity(bool saveToCache = true)
    {
        var link = _linksService.GetAllLinks().Links.FirstOrDefault(x => x.Name?.Contains("TAUTULLI", StringComparison.OrdinalIgnoreCase) == true);
        if (link?.Identifier is not Guid linkIdentifier)
            return new PlexActivityResponse();

        try
        {
            var apiKey = Uri.EscapeDataString(SettingsService.ResolveValue("ASPNETCORE_TAUTULLI_API_KEY") ?? string.Empty);
            using var httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(2) };
            using var result = httpClient.GetAsync(
                $"http://{link.Host}:{link.Port}/api/v2?apikey={apiKey}&cmd=get_activity").Result;

            if (!result.IsSuccessStatusCode)
                return GetActivity();

            var response = result.Content.ReadAsStringAsync().Result;
            var activity = JsonConvert.DeserializeObject<PlexActivityResponse>(response);
            if (activity is null)
                return GetActivity();

            if (saveToCache)
                _widgetCache.Save(linkIdentifier, WidgetTypes.Plex, activity);

            return activity;
        }
        catch (Exception exception)
        {
            Console.WriteLine($"Failed to refresh Plex activity: {exception.Message}");
            return GetActivity();
        }
    }

    public void OnStarted()
    {
        _isStarted = true;

        Task.Run(() =>
        {
            var nextCacheRefresh = DateTime.MinValue;

            while (_isStarted)
            {
                var saveToCache = DateTime.UtcNow >= nextCacheRefresh;
                var activity = RefreshActivity(saveToCache);

                if (saveToCache)
                    nextCacheRefresh = DateTime.UtcNow.AddMinutes(15);

                WebSockets.WebSocketManager.Instance().SendToAllClients(WebSocketKey.PlexActivity, new
                {
                    Response = new
                    {
                        Data = new
                        {
                            Sessions = activity.Response?.Data?.Sessions.ConvertAll(x => new
                            {
                                User = x.User,
                                FullTitle = x.FullTitle,
                                State = x.State,
                                ProgressPercentage = x.ProgressPercentage,
                                ViewOffset = x.ViewOffset,
                                Duration = x.Duration,
                                VideoDecision = x.VideoDecision,
                                Live = x.Live
                            }).ToList()
                        }
                    }
                });

                Thread.Sleep(TimeSpan.FromSeconds(5));
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
