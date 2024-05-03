using Newtonsoft.Json;

namespace HomeBoxLanding.Api.Features.Sonarr.Types;

public class SonarrActivityResponse
{
    public int TotalNumberOfSeries { get; set; }
    public int TotalNumberOfQueuedEpisodes { get; set; }
    public int TotalNumberOfMissingEpisodes { get; set; }
}

public class SonarrSeries
{
    [JsonProperty("id")]
    public int Identifier { get; set; }
    
    [JsonProperty("title")]
    public string Title { get; set; }
    
    [JsonProperty("statistics")]
    public SonarrSeriesStatistics Statistics { get; set; }
}

public class SonarrSeriesStatistics
{
    [JsonProperty("sizeOnDisk")]
    public long SizeOnDisk { get; set; }
}

public class SonarrQueue
{
    [JsonProperty("totalRecords")]
    public int Total { get; set; }
}

public class SonarrMissing
{
    [JsonProperty("totalRecords")]
    public int Total { get; set; }
}