using Newtonsoft.Json;

namespace HomeBoxLanding.Api.Features.UptimeKuma.Types;

public class UptimeKumaActivityResponse
{
    public UptimeKumaActivityResponse()
    {
        Metrics = new List<UptimeKumaMetric>();
    }
    
    public List<UptimeKumaMetric> Metrics { get; set; }
}

public class UptimeKumaMetric
{
    public string Name { get; set; }
    public bool IsUp { get; set; }
}