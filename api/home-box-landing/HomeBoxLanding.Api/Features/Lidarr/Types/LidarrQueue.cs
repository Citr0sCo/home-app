using Newtonsoft.Json;

namespace HomeBoxLanding.Api.Features.Lidarr.Types;

public class LidarrQueue
{
    [JsonProperty("totalRecords")]
    public int Total { get; set; }
}