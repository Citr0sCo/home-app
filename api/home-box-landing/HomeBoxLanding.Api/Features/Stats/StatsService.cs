using System.Diagnostics;
using System.Reflection;
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
            return new StatsResponse
            {
                CpuUsage = GetCpuUsageForProcess(),
                MemoryUsage = GetMemoryUsageForProcess(),
                DiskUsage = GetDiskUsageForProcess()
            };
        }
        private Stat GetCpuUsageForProcess()
        {
            var startTime = DateTime.UtcNow;
            var startCpuUsage = Process.GetCurrentProcess().TotalProcessorTime;
            Thread.Sleep(500);
    
            var endTime = DateTime.UtcNow;
            var endCpuUsage = Process.GetCurrentProcess().TotalProcessorTime;
            var cpuUsedMs = (endCpuUsage - startCpuUsage).TotalMilliseconds;
            var totalMsPassed = (endTime - startTime).TotalMilliseconds;
            var cpuUsageTotal = cpuUsedMs / (Environment.ProcessorCount * totalMsPassed);

            return new Stat
            {
                Percentage = Math.Round(cpuUsageTotal * 100, 2)
            };
        }
        private Stat GetMemoryUsageForProcess()
        {
            var output = _shellService.Run("free -m");
            
            var lines = output.Split("\n");

            if (lines.Length < 2)
                return new Stat();
            
            var memory = lines[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);

            if (memory.Length < 2)
                return new Stat();
      
            return new Stat
            {
                Total = double.Parse(memory[1]),
                Used = double.Parse(memory[2]),
                Percentage = Math.Round(double.Parse(memory[2]) / double.Parse(memory[1]), 2)
            };
        }
        private Stat GetDiskUsageForProcess()
        {
            var driveInfo = new DriveInfo(AppContext.BaseDirectory);
            double totalDriveSize = driveInfo.TotalSize;
            double usedDriveSize = driveInfo.TotalSize - driveInfo.AvailableFreeSpace;
            
            return new Stat
            {
                Percentage = Math.Round(usedDriveSize / totalDriveSize, 2), 
                Total = driveInfo.TotalSize,
                Used = driveInfo.TotalSize - driveInfo.AvailableFreeSpace
            };
        }
    }
}