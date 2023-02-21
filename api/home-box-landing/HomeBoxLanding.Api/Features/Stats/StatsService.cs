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
            var cpuUsage = _shellService.Run("grep 'cpu ' /proc/stat | awk '{print ($2+$4)*100/($2+$4+$5)} END {print usage}'");
            var memoryUsage = _shellService.Run("free");
            var diskUsage = _shellService.Run("df -h");

            return new StatsResponse
            {
                CpuUsage = new Stat { Percentage = double.Parse(cpuUsage), Raw = cpuUsage },
                MemoryUsage = new Stat { /*Percentage = double.Parse(memoryUsage),*/ Raw = memoryUsage },
                DiskUsage = new Stat { /*Percentage = double.Parse(diskUsage),*/ Raw = diskUsage }
            };
        }
    }
}