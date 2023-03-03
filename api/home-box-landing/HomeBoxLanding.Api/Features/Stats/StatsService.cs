using HomeBoxLanding.Api.Core.Events.Types;
using HomeBoxLanding.Api.Core.Shell;
using HomeBoxLanding.Api.Core.Types;
using HomeBoxLanding.Api.Features.Stats.Types;
using HomeBoxLanding.Api.Features.WebSockets.Types;

namespace HomeBoxLanding.Api.Features.Stats
{
    public class StatsService : ISubscriber
    {
        private readonly IShellService _shellService;
        private bool _isStarted = false;

        public StatsService(IShellService shellService)
        {
            _shellService = shellService;
        }

        public StatsResponse GetServerStats()
        {
            var response = new StatsResponse();

            var output = _shellService.RunOnHost("docker stats home-app --no-stream");

            var lines = output.Split("\n");

            if (lines.Length < 2)
            {
                return new StatsResponse
                {
                    HasError = true,
                    Error = new Error
                    {
                        Code = ErrorCode.FailedToGetStats,
                        UserMessage = "Incorrect number of lines received from shell",
                        TechnicalMessage = $"Received the following: {output}"
                    }
                };
            }

            var stats = lines[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);

            if (stats.Length < 2)
            {
                return new StatsResponse
                {
                    HasError = true,
                    Error = new Error
                    {
                        Code = ErrorCode.FailedToGetStats,
                        UserMessage = "Incorrect number of lines received from shell when parsing stats",
                        TechnicalMessage = $"Received the following: {lines[1]}"
                    }
                };
            }

            response.CpuUsage = new Stat
            {
                Percentage = ParseSize(stats[2])
            };

            response.MemoryUsage = new Stat
            {
                Total = ParseSize(stats[5]),
                Used = ParseSize(stats[3]),
                Percentage = ParseSize(stats[6])
            };

            var driveInfo = new DriveInfo(AppContext.BaseDirectory);
            double totalDriveSize = driveInfo.TotalSize;
            double usedDriveSize = driveInfo.TotalSize - driveInfo.AvailableFreeSpace;

            response.DiskUsage = new Stat
            {
                Percentage = Math.Round(usedDriveSize / totalDriveSize, 2) * 100,
                Total = driveInfo.TotalSize,
                Used = driveInfo.TotalSize - driveInfo.AvailableFreeSpace
            };

            return response;
        }

        private static double ParseSize(string toRemove)
        {
            if (toRemove.Contains("%"))
                return Math.Round(double.Parse(toRemove.Replace("%", "")), 2, MidpointRounding.ToZero);

            if (toRemove.Contains("GiB"))
                return Math.Round(double.Parse(toRemove.Replace("GiB", "")) * 1024 * 1048576d, 2,
                    MidpointRounding.ToZero);

            if (toRemove.Contains("MiB"))
                return Math.Round(double.Parse(toRemove.Replace("MiB", "")) * 1048576d, 2, MidpointRounding.ToZero);

            return double.Parse(toRemove);
        }

        public void OnStarted()
        {
            _isStarted = true;

            Task.Run(() =>
            {
                while (_isStarted)
                {
                    WebSockets.WebSocketManager.Instance().SendToAllClients(WebSocketKey.ServerStats, GetServerStats());

                    Thread.Sleep(5000);
                }
            });
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