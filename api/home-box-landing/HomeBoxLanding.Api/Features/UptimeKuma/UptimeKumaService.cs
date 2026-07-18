using System.Net.Http.Headers;
using System.Text;
using Fennel.CSharp;
using HomeBoxLanding.Api.Core.Events.Types;
using HomeBoxLanding.Api.Features.Links;
using HomeBoxLanding.Api.Features.UptimeKuma.Types;
using HomeBoxLanding.Api.Features.WebSockets.Types;
using Newtonsoft.Json;

namespace HomeBoxLanding.Api.Features.UptimeKuma;

public class UptimeKumaService : ISubscriber
{
    private readonly LinksService _linksService;
    private bool _isStarted = false;

    public UptimeKumaService(LinksService linksService)
    {
        _linksService = linksService;
    }

    public async Task<UptimeKumaActivityResponse> GetActivity(Guid identifier)
    {
        var link = _linksService.GetAllLinks().Links.FirstOrDefault(x => x.Identifier == identifier);

        if (link == null)
            return new UptimeKumaActivityResponse();

        var baseUrl = $"http://{link.Host}:{link.Port}";
        var apiKey = Environment.GetEnvironmentVariable("ASPNETCORE_UPTIME_KUMA_API_KEY");
        var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($":{apiKey}"));
        
        var httpClient = new HttpClient();
        httpClient.Timeout = TimeSpan.FromSeconds(20);
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
        var result = await httpClient.GetAsync($"{baseUrl}/metrics").ConfigureAwait(false);
        var response = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

        var parsedResponse = new UptimeKumaActivityResponse();
        
        try
        {
            var lines = Prometheus.ParseText(response);

            foreach (var line in lines)
            {
                if (line.IsMetric)
                {
                    var metric = (Metric)line;
                    
                    if(metric.MetricName != "monitor_status")
                        continue;
                    
                    parsedResponse.Metrics.Add(new UptimeKumaMetric
                    {
                        Name = metric.Labels["monitor_name"],
                        IsUp = metric.MetricValue == 1
                    });
                }
            }
        }
        catch (Exception)
        {
            return new UptimeKumaActivityResponse();
        }

        parsedResponse.Metrics = parsedResponse.Metrics.OrderBy(x => x.Name).ToList();
        return parsedResponse;
    }
    
    private void DeleteSession(string baseUrl, string? sessionId)
    {
        if (sessionId == null)
            return;
        
        var httpClient = new HttpClient();
        httpClient.Timeout = TimeSpan.FromSeconds(20);
        httpClient.DefaultRequestHeaders.Add("sid", sessionId);
        var result = httpClient.DeleteAsync($"{baseUrl}/api/auth").Result;
        var response = result.Content.ReadAsStringAsync().Result;

        UptimeKumaActivityResponse? parsedResponse;
        
        try
        {
            parsedResponse = JsonConvert.DeserializeObject<UptimeKumaActivityResponse>(response);
        }
        catch (Exception)
        {
            return;
        }
    }

    public void OnStarted()
    {
        _isStarted = true;

        Task.Run(async () =>
        {
            while (_isStarted)
            {
                var linkService = new LinksService(new LinksRepository());
                var uptimeKumaLinks = linkService.GetAllLinks().Links.Where(x => x.Name.ToUpper().Contains("UPTIME KUMA"));
                
                var activities = new Dictionary<Guid, UptimeKumaActivityResponse>();
                
                foreach (var link in uptimeKumaLinks)
                {
                    activities.Add(link.Identifier!.Value, await GetActivity(link.Identifier!.Value));    
                }
                    
                WebSockets.WebSocketManager.Instance().SendToAllClients(WebSocketKey.UptimeKumaActivity, new
                {
                    Response = new
                    {
                        Data = new
                        {
                            Activities = activities.Select(x => new
                            {
                                Metrics = x.Value.Metrics.Select(y => new
                                {
                                    Name = y.Name,
                                    IsUp = y.IsUp
                                }).ToList()
                            }).ToList()
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