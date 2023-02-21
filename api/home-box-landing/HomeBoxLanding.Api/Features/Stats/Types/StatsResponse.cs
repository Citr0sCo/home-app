namespace HomeBoxLanding.Api.Features.Stats.Types
{
    public class StatsResponse
    {
        public Stat CpuUsage { get; set; }
        public Stat MemoryUsage { get; set; }
        public Stat DiskUsage { get; set; }
    }

    public class Stat
    {
        public double Percentage { get; set; }
        public double Total { get; set; }
        public double Used { get; set; }
    }
}