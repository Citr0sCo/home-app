using HomeBoxLanding.Api.Core.Events.Types;
using HomeBoxLanding.Api.Features.Links;
using HomeBoxLanding.Api.Features.Pihole.Types;
using HomeBoxLanding.Api.Features.WebSockets.Types;
using Minio;
using Newtonsoft.Json;

namespace HomeBoxLanding.Api.Features.Pihole;

public class PiholeService : ISubscriber
{
    private readonly LinksService _linksService;
    private bool _isStarted = false;
    private const string API_KEY = "efce54cc4fcf662aa3b425956ec85631f05c8a0df4d2a825ae8022b9a4824b46";

    public PiholeService(LinksService linksService)
    {
        _linksService = linksService;
    }

    public PiholeActivityResponse GetActivity(Guid identifier)
    {
        var link = _linksService.GetAllLinks().Links.FirstOrDefault(x => x.Identifier == identifier);

        if (link == null)
        {
            return new PiholeActivityResponse();
        }
        
        var httpClient = new HttpClient();
        httpClient.Timeout = TimeSpan.FromSeconds(20);
        var result = httpClient.GetAsync($"{link.Url}api.php?summary&auth={API_KEY}").Result;
        var response = result.Content.ReadAsStringAsync().Result;

        PiholeActivityResponse? parsedResponse;
        
        try
        {
            parsedResponse = JsonConvert.DeserializeObject<PiholeActivityResponse>(response);
        }
        catch (Exception)
        {
            return new PiholeActivityResponse();
        }

        if (parsedResponse == null)
        {
            return new PiholeActivityResponse();
        }

        parsedResponse.Identifier = identifier;
        
        return parsedResponse;
    }

    public void OnStarted()
    {
        _isStarted = true;

        Task.Run(() =>
        {
            while (_isStarted)
            {
                var linkService = new LinksService(new LinksRepository(), new MinioClient());
                var piholeLinks = linkService.GetAllLinks().Links.Where(x => x.Name.ToUpper().Contains("PIHOLE"));
                
                var activities = new Dictionary<Guid, PiholeActivityResponse>();
                
                foreach (var link in piholeLinks)
                {
                    activities.Add(link.Identifier!.Value, GetActivity(link.Identifier!.Value));    
                }
                    
                WebSockets.WebSocketManager.Instance().SendToAllClients(WebSocketKey.PiholeActivity, new
                {
                    Response = new
                    {
                        Data = new
                        {
                            Activities = activities.Select(x => new
                            {
                                Identifier = x.Key,
                                QueriesToday = x.Value.QueriesToday,
                                BlockedToday = x.Value.BlockedToday,
                                BlockedPercentage = x.Value.BlockedPercentage,
                                Clients = x.Value.Clients
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