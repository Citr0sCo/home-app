namespace HomeBoxLanding.Api.Features.Lidarr.Types;

public class LidarrActivityResponse
{
    public LidarrActivityResponse()
    {
        Health = new List<LidarrHealth>();
    }
    
    public int TotalNumberOfTracks { get; set; }
    public int TotalNumberOfQueuedTracks { get; set; }
    public int TotalMissingTracks { get; set; }
    public List<LidarrHealth> Health { get; set; }
}