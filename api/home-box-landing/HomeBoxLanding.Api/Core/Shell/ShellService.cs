using System.Diagnostics;

namespace HomeBoxLanding.Api.Core.Shell;

public interface IShellService
{
    string Run(string command);
    string RunOnHost(string command);
}

public class ShellService : IShellService
{
    private static ShellService? _instance;
    private static readonly object _hostCommandLock = new();

    private ShellService()
    {
            
    }

    public static ShellService Instance()
    {
        if (_instance == null)
            _instance = new ShellService();

        return _instance;
    }

    public string Run(string command)
    {
        var escapedArgs = $"{command.Replace("\"", "\\\"")}";

        var info = new ProcessStartInfo
        {
            FileName = "/bin/bash",
            Arguments = $"-c \"{escapedArgs}\"",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
            
        using (var process = Process.Start(info))
        {
            return process!.StandardOutput.ReadToEnd();
        }
    }

    public string RunOnHost(string command)
    {
        lock (_hostCommandLock)
        {
            var marker = $"__HOME_APP_COMMAND_COMPLETE_{Guid.NewGuid():N}__";
            var commandWithMarker = $"{command}; printf '\\n{marker}\\n'";
            var escapedArgs = $"echo \\\"{commandWithMarker.Replace("\"", "\\\"")}\\\" > /host/pipe";
            var info = new ProcessStartInfo
            {
                FileName = "/bin/bash",
                Arguments = $"-c \"{escapedArgs}\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(info);
            process?.WaitForExit();

            var timeout = Stopwatch.StartNew();
            while (timeout.Elapsed < TimeSpan.FromSeconds(30))
            {
                try
                {
                    var output = File.ReadAllText("/host/pipe_log.txt");
                    var markerIndex = output.IndexOf(marker, StringComparison.Ordinal);
                    if (markerIndex >= 0)
                        return output[..markerIndex].TrimEnd();
                }
                catch (IOException)
                {
                    // The host command runner may still be writing the log.
                }

                Thread.Sleep(50);
            }

            throw new TimeoutException("Timed out waiting for the host command to complete.");
        }
    }
}
