using System.Diagnostics;

namespace HomeBoxLanding.Api.Core.Shell;

public interface IShellService
{
    string Run(string command);
    string RunOnHost(string command);
    string RunOnHostSecondary(string command);
}

public class ShellService : IShellService
{
    private static ShellService? _instance;
    private static bool _hasOngoingTask = false;
    private static bool _hasOngoingSecondaryTask = false;

    private ShellService()
    {
            
    }

    public static ShellService Instance()
    {
        if (_instance == null)
            _instance = new ShellService();

        return _instance;
    }

    public string RunOnHost(string command)
    {
        while(_hasOngoingTask)
            Thread.Sleep(1000);
        
        _hasOngoingTask = true;
        
        var escapedArgs = $"echo \\\"{command.Replace("\"", "\\\"")}\\\" > /host/pipe";

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
            process?.WaitForExitAsync();
            var result = File.ReadAllTextAsync("/host/pipe_log.txt").Result;
            
            _hasOngoingTask = false;
            
            return result;
        }
    }

    public string RunOnHostSecondary(string command)
    {
        while(_hasOngoingSecondaryTask)
            Thread.Sleep(1000);
        
        _hasOngoingSecondaryTask = true;
        
        var escapedArgs = $"echo \\\"{command.Replace("\"", "\\\"")}\\\" > /host/pipe_secondary";

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
            process?.WaitForExitAsync();
            var result = File.ReadAllTextAsync("/host/pipe_secondary_log.txt").Result;
            
            _hasOngoingSecondaryTask = false;
            
            return result;
        }
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
            return process.StandardOutput.ReadToEnd();
        }
    }
}