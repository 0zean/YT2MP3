using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using System.Runtime.InteropServices;
using WinRT;

namespace YT2MP3
{
    public sealed partial class DownloadPage : Page
    {
        private DownloadService _downloadService;

        public DownloadPage()
        {
            this.InitializeComponent();
            _downloadService = new DownloadService();
        }

        private async void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var folderPicker = new Windows.Storage.Pickers.FolderPicker();
            folderPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Downloads;
            folderPicker.FileTypeFilter.Add("*");

            // Initialize the folder picker with the window handle
            WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, WindowHelper.GetActiveWindowHandle());

            var folder = await folderPicker.PickSingleFolderAsync();
            if (folder != null)
            {
                DestinationTextBox.Text = folder.Path;
            }
        }

        private async void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            string url = YouTubeUrlTextBox.Text;
            string outputPath = DestinationTextBox.Text;
            bool isPlaylist = PlaylistCheckBox.IsChecked ?? false;
            string format = SettingsManager.PreferredFormat;

            if (string.IsNullOrWhiteSpace(url) || string.IsNullOrWhiteSpace(outputPath))
            {
                await ShowErrorDialog("Please enter a valid URL and destination.");
                return;
            }

            bool success = await _downloadService.DownloadAudio(url, outputPath, isPlaylist, format);

            if (success)
            {
                await ShowSuccessDialog("Download completed successfully.");
            }
            else
            {
                await ShowErrorDialog("An error occurred during the download.");
            }
        }

        private async Task ShowErrorDialog(string message)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Error",
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };

            await dialog.ShowAsync();
        }

        private async Task ShowSuccessDialog(string message)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Success",
                Content = message,
                CloseButtonText = "OK"
            };

            await dialog.ShowAsync();
        }

        private static class WindowHelper
        {
            [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto, PreserveSig = true, SetLastError = false)]
            public static extern IntPtr GetActiveWindow();

            public static IntPtr GetActiveWindowHandle()
            {
                return GetActiveWindow();
            }
        }
    }
}
