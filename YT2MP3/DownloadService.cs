using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace YT2MP3
{
    public class DownloadService
    {
        public async Task<bool> DownloadAudio(string url, string outputPath, bool isPlaylist, string format)
        {
            string ytDlpPath = GetYtDlpPath();
            if (string.IsNullOrEmpty(ytDlpPath))
            {
                throw new FileNotFoundException("yt-dlp.exe not found in PATH");
            }

            string arguments = $"-x --audio-format {format} -o \"{Path.Combine(outputPath, "%(title)s.%(ext)s")}\" --format bestaudio --extract-audio ";

            if (isPlaylist)
            {
                arguments += "--yes-playlist ";
            }
            else
            {
                arguments += "--no-playlist ";
            }

            arguments += url;

            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = ytDlpPath,
                        Arguments = arguments,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                await process.WaitForExitAsync();

                return process.ExitCode == 0;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }

        private string GetYtDlpPath()
        {
            string[] paths = Environment.GetEnvironmentVariable("PATH").Split(Path.PathSeparator);
            foreach (string path in paths)
            {
                string fullPath = Path.Combine(path, "yt-dlp.exe");
                if (File.Exists(fullPath))
                {
                    return fullPath;
                }
            }
            return null;
        }
    }
}