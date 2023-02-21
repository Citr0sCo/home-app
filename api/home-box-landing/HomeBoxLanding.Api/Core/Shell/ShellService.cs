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
            process.WaitForExit();
            
            return File.ReadAllText("/host/pipe_log.txt");
        }
    }
}