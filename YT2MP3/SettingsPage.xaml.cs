using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage.Pickers;
using System.Runtime.InteropServices;
using WinRT;

namespace YT2MP3
{
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();
            LoadSettings();
        }

        private void LoadSettings()
        {
            PreferredFormatComboBox.SelectedValue = SettingsManager.PreferredFormat;
            DefaultSaveLocationTextBox.Text = SettingsManager.DefaultSaveLocation;
            AppThemeComboBox.SelectedValue = SettingsManager.AppTheme.ToString();
        }

        private async void BrowseDefaultLocationButton_Click(object sender, RoutedEventArgs e)
        {
            var folderPicker = new Windows.Storage.Pickers.FolderPicker();
            folderPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Downloads;
            folderPicker.FileTypeFilter.Add("*");

            // Initialize the folder picker with the window handle
            WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, WindowHelper.GetActiveWindowHandle());

            var folder = await folderPicker.PickSingleFolderAsync();
            if (folder != null)
            {
                DefaultSaveLocationTextBox.Text = folder.Path;
            }
        }

        private void SaveSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsManager.PreferredFormat = (string)PreferredFormatComboBox.SelectedValue;
            SettingsManager.DefaultSaveLocation = DefaultSaveLocationTextBox.Text;
            SettingsManager.AppTheme = (ElementTheme)Enum.Parse(typeof(ElementTheme), (string)AppThemeComboBox.SelectedValue);

            SettingsManager.ApplyTheme(App.MainWindow);

            // Show a confirmation message
            var dialog = new ContentDialog
            {
                Title = "Settings Saved",
                Content = "Your settings have been saved successfully.",
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };

            _ = dialog.ShowAsync();
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
