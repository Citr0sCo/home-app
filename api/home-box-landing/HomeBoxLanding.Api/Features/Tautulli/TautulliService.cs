using HomeBoxLanding.Api.Features.Settings;
using HomeBoxLanding.Api.Core.Events.Types;
using HomeBoxLanding.Api.Features.Links;
using HomeBoxLanding.Api.Features.Tautulli.Types;
using HomeBoxLanding.Api.Features.WebSockets.Types;
using Newtonsoft.Json.Linq;

namespace HomeBoxLanding.Api.Features.Tautulli;

public class TautulliService : ISubscriber
{
    private readonly LinksService _linksService;
    private bool _isStarted;

    public TautulliService(LinksService linksService)
    {
        _linksService = linksService;
    }

    public TautulliStatsResponse GetStats(Guid identifier)
    {
        var stats = new TautulliStatsResponse { Identifier = identifier };
        var link = _linksService.GetAllLinks().Links.FirstOrDefault(x => x.Identifier == identifier);

        if (link?.Host == null || link.Port <= 0)
            return stats;

        var baseUrl = $"http://{link.Host}:{link.Port}";
        var libraries = GetData(baseUrl, "get_libraries");
        var users = GetData(baseUrl, "get_users");

        if (libraries is JArray libraryItems)
        {
            foreach (var library in libraryItems)
            {
                var sectionType = library["section_type"]?.Value<string>();
                var count = library["count"]?.ToObject<int>() ?? 0;

                if (string.Equals(sectionType, "movie", StringComparison.OrdinalIgnoreCase))
                    stats.TotalMovies += count;
                else if (string.Equals(sectionType, "show", StringComparison.OrdinalIgnoreCase))
                    stats.TotalShows += count;
            }
        }

        if (users is JArray userItems)
            stats.TotalUsers = userItems.Count;

        return stats;
    }

    private static JToken? GetData(string baseUrl, string command)
    {
        try
        {
            var apiKey = SettingsService.ResolveValue("ASPNETCORE_TAUTULLI_API_KEY");
            var url = $"{baseUrl}/api/v2?apikey={Uri.EscapeDataString(apiKey ?? string.Empty)}&cmd={command}";
            using var httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(20) };
            using var result = httpClient.GetAsync(url).Result;

            if (!result.IsSuccessStatusCode)
                return null;

            var response = JObject.Parse(result.Content.ReadAsStringAsync().Result);
            var responseObject = response["response"];

            if (!string.Equals(responseObject?["result"]?.Value<string>(), "success", StringComparison.OrdinalIgnoreCase))
                return null;

            return responseObject?["data"];
        }
        catch (Exception)
        {
            return null;
        }
    }

    public void OnStarted()
    {
        _isStarted = true;

        Task.Run(() =>
        {
            while (_isStarted)
            {
                var tautulliLinks = new LinksService(new LinksRepository())
                    .GetAllLinks().Links
                    .Where(x => x.Name?.Contains("TAUTULLI", StringComparison.OrdinalIgnoreCase) == true && x.Identifier.HasValue);

                var stats = tautulliLinks
                    .Select(x => GetStats(x.Identifier!.Value))
                    .ToList();

                WebSockets.WebSocketManager.Instance().SendToAllClients(WebSocketKey.TautulliStats, new
                {
                    Response = new
                    {
                        Data = new
                        {
                            Activities = stats.Select(x => new
                            {
                                Identifier = x.Identifier,
                                TotalMovies = x.TotalMovies,
                                TotalShows = x.TotalShows,
                                TotalUsers = x.TotalUsers
                            }).ToList()
                        }
                    }
                });

                Thread.Sleep(TimeSpan.FromSeconds(15));
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
