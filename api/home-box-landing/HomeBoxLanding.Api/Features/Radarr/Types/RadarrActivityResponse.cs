using Newtonsoft.Json;

namespace HomeBoxLanding.Api.Features.Radarr.Types;

public class RadarrActivityResponse
{
    public int TotalNumberOfMovies { get; set; }
    public int TotalNumberOfQueuedMovies { get; set; }
    public int TotalMissingMovies { get; set; }
}

public class RadarrMovie
{
    [JsonProperty("id")]
    public int Identifier { get; set; }
    
    [JsonProperty("title")]
    public string Title { get; set; }
    
    [JsonProperty("sizeOnDisk")]
    public long SizeOnDisk { get; set; }
}

public class RadarrQueue
{
    [JsonProperty("totalRecords")]
    public int Total { get; set; }
}