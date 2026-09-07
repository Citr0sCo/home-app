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

    public PlexActivityResponse RefreshActivity()
    {
        var link = _linksService.GetAllLinks().Links.FirstOrDefault(x => x.Name?.Contains("TAUTULLI", StringComparison.OrdinalIgnoreCase) == true);
        if (link?.Identifier is not Guid linkIdentifier)
            return new PlexActivityResponse();

        using var httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(2) };
        using var result = httpClient.GetAsync($"http://{link.Host}:{link.Port}/api/v2?apikey={SettingsService.ResolveValue("ASPNETCORE_TAUTULLI_API_KEY")}&cmd=get_activity").Result;
        var response = result.Content.ReadAsStringAsync().Result;
        var activity = JsonConvert.DeserializeObject<PlexActivityResponse>(response) ?? new PlexActivityResponse();
        _widgetCache.Save(linkIdentifier, WidgetTypes.Plex, activity);
        return activity;
    }

    public void OnStarted()
    {
        _isStarted = true;

        Task.Run(() =>
        {
            while (_isStarted)
            {
                var activity = RefreshActivity();

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
                                VideoDecision = x.VideoDecision
                            }).ToList()
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
