using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.IO;

namespace YT2MP3
{
    public static class DependencyChecker
    {
        public static async Task<bool> CheckAndInstallDependencies(XamlRoot xamlRoot)
        {
            bool ytDlpInstalled = CheckYtDlp();
            bool ffmpegInstalled = CheckFfmpeg();

            if (!ytDlpInstalled || !ffmpegInstalled)
            {
                return await PromptForInstallation(xamlRoot, !ytDlpInstalled, !ffmpegInstalled);
            }

            return true;
        }

        private static bool CheckYtDlp()
        {
            string[] paths = Environment.GetEnvironmentVariable("PATH").Split(Path.PathSeparator);
            foreach (string path in paths)
            {
                if (File.Exists(Path.Combine(path, "yt-dlp.exe")))
                {
                    return true;
                }
            }
            return false;
        }

        private static bool CheckFfmpeg()
        {
            string[] paths = Environment.GetEnvironmentVariable("PATH").Split(Path.PathSeparator);
            foreach (string path in paths)
            {
                if (File.Exists(Path.Combine(path, "ffmpeg.exe")))
                {
                    return true;
                }
            }
            return false;
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

        private static async Task<bool> PromptForInstallation(XamlRoot xamlRoot, bool needYtDlp, bool needFfmpeg)
        {
            string message = "The following dependencies are missing:\n";
            if (needYtDlp) message += "- yt-dlp\n";
            if (needFfmpeg) message += "- ffmpeg\n";
            message += "\nWould you like to install them now?";

            ContentDialog dialog = new ContentDialog
            {
                Title = "Missing Dependencies",
                Content = message,
                PrimaryButtonText = "Install",
                CloseButtonText = "Cancel",
                XamlRoot = xamlRoot
            };

            ContentDialogResult result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                // Here you would implement the actual installation process
                // For now, we'll just show a message
                ContentDialog installDialog = new ContentDialog
                {
                    Title = "Installation",
                    Content = "Installing dependencies... (This is a placeholder, actual installation not implemented)",
                    CloseButtonText = "OK",
                    XamlRoot = xamlRoot
                };

                await installDialog.ShowAsync();

                ContentDialog restartDialog = new ContentDialog
                {
                    Title = "Restart Required",
                    Content = "The application needs to be restarted for the changes to take effect. Please restart the application.",
                    CloseButtonText = "OK",
                    XamlRoot = xamlRoot
                };

                await restartDialog.ShowAsync();

                // Return true assuming installation was successful
                return true;
            }

            return false;
        }
    }
}
