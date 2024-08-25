using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;

namespace YT2MP3
{
    public class DownloadService
    {
        public async Task<bool> DownloadAudio(string url, string outputPath, bool isPlaylist, string format)
        {
            string arguments = $"-x --audio-format {format} -o \"{Path.Combine(outputPath, "%(title)s.%(ext)s")}\" ";

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
                        FileName = "yt-dlp",
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
    }
}