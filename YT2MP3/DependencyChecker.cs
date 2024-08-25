using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace YT2MP3
{
    public static class DependencyChecker
    {
        public static async Task<bool> CheckAndInstallDependencies()
        {
            bool ytDlpInstalled = await CheckYtDlp();
            bool ffmpegInstalled = await CheckFfmpeg();

            if (!ytDlpInstalled || !ffmpegInstalled)
            {
                // Prompt user to install missing dependencies
                // For simplicity, we'll just return false here
                // In a real application, you'd show a dialog and handle the installation process
                return false;
            }

            return true;
        }

        private static async Task<bool> CheckYtDlp()
        {
            return await CheckCommand("yt-dlp", "--version");
        }

        private static async Task<bool> CheckFfmpeg()
        {
            return await CheckCommand("ffmpeg", "-version");
        }

        private static async Task<bool> CheckCommand(string command, string arguments)
        {
            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = command,
                        Arguments = arguments,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                await process.WaitForExitAsync();

                return process.ExitCode == 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
