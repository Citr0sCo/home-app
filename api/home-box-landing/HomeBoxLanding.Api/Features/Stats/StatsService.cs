using HomeBoxLanding.Api.Core.Shell;
using HomeBoxLanding.Api.Features.Stats.Types;

namespace HomeBoxLanding.Api.Features.Stats
{
    public class StatsService
    {
        private readonly ShellService _shellService;

        public StatsService(ShellService shellService)
        {
            _shellService = shellService;
        }

        public StatsResponse GetServerStats()
        {
            var cpuAndMemoryRaw = _shellService.Run("top -bn1");
            var diskUsage = _shellService.Run("df -h");

            return new StatsResponse
            {
                CpuUsage = new Stat { /*Percentage = double.Parse(cpuUsage),*/ Raw = cpuAndMemoryRaw },
                MemoryUsage = new Stat { /*Percentage = double.Parse(memoryUsage),*/ Raw = cpuAndMemoryRaw },
                DiskUsage = new Stat { /*Percentage = double.Parse(diskUsage),*/ Raw = diskUsage }
            };
        }
    }
}