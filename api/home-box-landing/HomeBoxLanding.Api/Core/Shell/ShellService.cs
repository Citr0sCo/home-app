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
            var currentOutput =  File.ReadAllText("/host/pipe_log.txt");
            
            if(currentOutput.Length != 0)
                File.WriteAllText("/host/pipe_log.txt", "");
            
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
            
            var result = File.ReadAllText("/host/pipe_log.txt");
            File.WriteAllText("/host/pipe_log.txt", "");
            return result;
        }
    }
}