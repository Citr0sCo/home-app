namespace HomeBoxLanding.Api.Features.Lidarr.Types;

public class LidarrActivityResponse
{
    public LidarrActivityResponse()
    {
        Health = new List<LidarrHealth>();
    }
    
    public int TotalNumberOfMovies { get; set; }
    public int TotalNumberOfQueuedMovies { get; set; }
    public int TotalMissingMovies { get; set; }
    public List<LidarrHealth> Health { get; set; }
}