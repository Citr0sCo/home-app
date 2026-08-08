using System.ComponentModel.DataAnnotations;

namespace HomeBoxLanding.Api.Features.Stats.Types;

public class ServerStatsHistoryRecord
{
    [Key]
    public Guid Identifier { get; set; }
    public DateTime RecordedAt { get; set; }
    public double CpuPercentage { get; set; }
    public double MemoryPercentage { get; set; }
    public double MemoryUsed { get; set; }
    public double MemoryTotal { get; set; }
    public double DiskPercentage { get; set; }
    public double DiskUsed { get; set; }
    public double DiskTotal { get; set; }
}
