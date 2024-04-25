using Newtonsoft.Json;

namespace HomeBoxLanding.Api.Features.Pihole.Types;

public class PiholeActivityResponse
{
    public Guid? Identifier { get; set; }
    
    [JsonProperty("dns_queries_today")]
    public string QueriesToday { get; set; }
    
    [JsonProperty("ads_blocked_today")]
    public string BlockedToday { get; set; }
    
    [JsonProperty("ads_percentage_today")]
    public string BlockedPercentage { get; set; }
    
    [JsonProperty("unique_clients")]
    public string Clients { get; set; }
}