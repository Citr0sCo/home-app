using System.Diagnostics;
using HomeBoxLanding.Api.Core.Events.Types;
using HomeBoxLanding.Api.Core.Shell;
using HomeBoxLanding.Api.Features.Stats.Types;
using HomeBoxLanding.Api.Features.WebSockets.Types;

namespace HomeBoxLanding.Api.Features.Stats
{
    public class StatsService : ISubscriber
    {
        private readonly ShellService _shellService;
        private bool _isStarted = false;

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

        public void OnStarted()
        {
            _isStarted = true;

            while (_isStarted)
            {
                WebSockets.WebSocketManager.Instance().SendToAllClients(WebSocketKey.ServerStats, new {
                    CpuUsage = GetCpuUsageForProcess(),
                    MemoryUsage = GetMemoryUsageForProcess(),
                    DiskUsage = GetDiskUsageForProcess()
                });
                
                Thread.Sleep(5000);
            }
        }

        public void OnStopping()
        {
            _isStarted = false;
        }

        public void OnStopped()
        {
            // Do nothing
        }
    }
}