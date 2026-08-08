using HomeBoxLanding.Api.Core.Types;

namespace HomeBoxLanding.Api.Features.Stats.Types;

public class ServerStatsHistoryResponse : CommunicationResponse
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public List<ServerStatsHistoryPoint> Samples { get; set; } = new();
}

public class ServerStatsHistoryPoint
{
    public DateTime RecordedAt { get; set; }
    public double CpuPercentage { get; set; }
    public double MemoryPercentage { get; set; }
    public double MemoryUsed { get; set; }
    public double MemoryTotal { get; set; }
    public double DiskPercentage { get; set; }
    public double DiskUsed { get; set; }
    public double DiskTotal { get; set; }
}
