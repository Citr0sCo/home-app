using Newtonsoft.Json;

namespace HomeBoxLanding.Api.Features.Lidarr.Types;

public class LidarrMovie
{
    [JsonProperty("id")]
    public int Identifier { get; set; }
    
    [JsonProperty("title")]
    public string Title { get; set; }
    
    [JsonProperty("sizeOnDisk")]
    public long SizeOnDisk { get; set; }
}