using System.Diagnostics;

namespace home_box_landing.api.Core.Shell
{
    public interface IShellService
    {
        string Run(string command);
    }

    public class ShellService : IShellService
    {
        public string Run(string command)
        {
            var escapedArgs = command.Replace("\"", "\\\"");
        
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
            var result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            
            return result;
        }
    }
}