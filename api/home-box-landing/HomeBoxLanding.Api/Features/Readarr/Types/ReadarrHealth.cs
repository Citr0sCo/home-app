using Newtonsoft.Json;

namespace HomeBoxLanding.Api.Features.Readarr.Types;

public class ReadarrHealth
{
    [JsonProperty("type")]
    public string Type { get; set; }
        
    [JsonProperty("message")]
    public string Message { get; set; }
        
    [JsonProperty("wikiUrl")]
    public string WikiUrl { get; set; }
        
    [JsonProperty("source")]
    public string Source { get; set; }
}