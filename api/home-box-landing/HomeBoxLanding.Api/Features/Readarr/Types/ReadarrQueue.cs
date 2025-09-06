using Newtonsoft.Json;

namespace HomeBoxLanding.Api.Features.Readarr.Types;

public class ReadarrQueue
{
    [JsonProperty("totalRecords")]
    public int Total { get; set; }
}