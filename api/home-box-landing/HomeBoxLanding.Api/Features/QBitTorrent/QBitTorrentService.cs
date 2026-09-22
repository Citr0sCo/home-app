using HomeBoxLanding.Api.Core.Events.Types;
using HomeBoxLanding.Api.Features.Links;
using HomeBoxLanding.Api.Features.QBitTorrent.Types;
using HomeBoxLanding.Api.Features.WebSockets.Types;
using Newtonsoft.Json.Linq;

namespace HomeBoxLanding.Api.Features.QBitTorrent;

public class QBitTorrentService : ISubscriber
{
    private readonly LinksService _linksService;
    private bool _isStarted;

    public QBitTorrentService(LinksService linksService)
    {
        _linksService = linksService;
    }

    public QBitTorrentStatsResponse GetStats(Guid identifier)
    {
        var stats = new QBitTorrentStatsResponse { Identifier = identifier };
        var link = _linksService.GetAllLinks().Links.FirstOrDefault(x => x.Identifier == identifier);

        if (link?.Host == null || link.Port <= 0)
            return stats;

        var scheme = link.IsSecure ? "https" : "http";
        var baseUrl = $"{scheme}://{link.Host}:{link.Port}";
        var torrents = GetTorrents(baseUrl);

        if (torrents == null)
            return stats;

        stats.TotalTorrents = torrents.Count;
        stats.UploadRate = torrents.Sum(torrent => torrent["upspeed"]?.Value<long>() ?? 0);
        stats.DownloadRate = torrents.Sum(torrent => torrent["dlspeed"]?.Value<long>() ?? 0);
        stats.TotalLeeches = torrents.Sum(torrent => torrent["num_leechs"]?.Value<int>() ?? 0);

        return stats;
    }

    public void OnStarted()
    {
        _isStarted = true;

        Task.Run(() =>
        {
            while (_isStarted)
            {
                var qBitTorrentLinks = new LinksService(new LinksRepository())
                    .GetAllLinks().Links
                    .Where(x => x.Name?.Contains("QBITTORRENT", StringComparison.OrdinalIgnoreCase) == true && x.Identifier.HasValue);

                var stats = qBitTorrentLinks
                    .Select(x => GetStats(x.Identifier!.Value))
                    .ToList();

                WebSockets.WebSocketManager.Instance().SendToAllClients(WebSocketKey.QBitTorrentStats, new
                {
                    Response = new
                    {
                        Data = new
                        {
                            Activities = stats.Select(x => new
                            {
                                Identifier = x.Identifier,
                                TotalTorrents = x.TotalTorrents,
                                UploadRate = x.UploadRate,
                                DownloadRate = x.DownloadRate,
                                TotalLeeches = x.TotalLeeches
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

    private static JArray? GetTorrents(string baseUrl)
    {
        try
        {
            using var httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(20) };
            using var result = httpClient.GetAsync($"{baseUrl}/api/v2/torrents/info").Result;

            if (!result.IsSuccessStatusCode)
                return null;

            return JArray.Parse(result.Content.ReadAsStringAsync().Result);
        }
        catch (Exception)
        {
            return null;
        }
    }
}
