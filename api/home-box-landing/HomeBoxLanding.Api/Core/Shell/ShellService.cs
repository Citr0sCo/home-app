using System.Diagnostics;

namespace HomeBoxLanding.Api.Core.Shell
{
    public interface IShellService
    {
        string Run(string command);
    }

    public class ShellService : IShellService
    {
        private static ShellService? _instance;

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
            var currentFileContents = File.ReadAllTextAsync("/host/pipe_log.txt");

            while (currentFileContents.Result.Length > 0)
            {
                Thread.Sleep(100);
                currentFileContents = File.ReadAllTextAsync("/host/pipe_log.txt");
            }

            var escapedArgs = $"echo \\\"{command.Replace("\"", "\\\"")}\\\" > /host/pipe";
        
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            
            process.Start();
            process.WaitForExitAsync();
            
            var result = File.ReadAllTextAsync("/host/pipe_log.txt").Result;
            File.WriteAllTextAsync("/host/pipe_log.txt", "");
            return result;
        }
    }
}