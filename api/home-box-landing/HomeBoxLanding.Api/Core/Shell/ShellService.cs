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
            var currentOutput = File.ReadAllText("/host/pipe_log.txt");

            while (!IsFileReady("/host/pipe_log.txt"))
                Thread.Sleep(100);
            
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
        
        public static bool IsFileReady(string filename)
        {
            try
            {
                using (var inputStream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.None))
                    return inputStream.Length > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}