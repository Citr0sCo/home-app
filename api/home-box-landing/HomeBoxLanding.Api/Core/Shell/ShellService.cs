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
            var escapedArgs = $"echo \\\"{command.Replace("\"", "\\\"")}\\\" > /host/pipe";
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
            return File.ReadAllText("/host/pipe_log.txt");
        }
    }
}