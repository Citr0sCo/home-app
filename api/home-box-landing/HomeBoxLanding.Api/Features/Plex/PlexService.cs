using HomeBoxLanding.Api.Core.Events.Types;
using HomeBoxLanding.Api.Features.Plex.Types;
using HomeBoxLanding.Api.Features.WebSockets.Types;
using Newtonsoft.Json;

namespace HomeBoxLanding.Api.Features.Plex;

public class PlexService : ISubscriber
{
    private bool _isStarted = false;

    public PlexActivityResponse GetActivity()
    {
        var httpClient = new HttpClient();
        httpClient.Timeout = TimeSpan.FromSeconds(2);
        var result = httpClient.GetAsync("http://192.168.1.161:8181/api/v2?apikey=ffMEl3ZwuKtatA5H8sfmCh0LbRTbmiQ3&cmd=get_activity").Result;
        var response = result.Content.ReadAsStringAsync().Result;
            
        return JsonConvert.DeserializeObject<PlexActivityResponse>(response) ?? new PlexActivityResponse();
    }

    public void OnStarted()
    {
        _isStarted = true;

        Task.Run(() =>
        {
            while (_isStarted)
            {
                var activity = GetActivity();
                    
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