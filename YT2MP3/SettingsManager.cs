using Microsoft.UI.Xaml;
using Windows.Storage;

namespace YT2MP3
{
    public static class SettingsManager
    {
        private static ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        public static string PreferredFormat
        {
            get => (string)localSettings.Values["PreferredFormat"] ?? "mp3";
            set => localSettings.Values["PreferredFormat"] = value;
        }

        public static string DefaultSaveLocation
        {
            get => (string)localSettings.Values["DefaultSaveLocation"] ?? "";
            set => localSettings.Values["DefaultSaveLocation"] = value;
        }

        public static ElementTheme AppTheme
        {
            get => (ElementTheme)((int?)localSettings.Values["AppTheme"] ?? (int)ElementTheme.Default);
            set => localSettings.Values["AppTheme"] = (int)value;
        }

        public static void ApplyTheme(Window window)
        {
            if (window.Content is FrameworkElement rootElement)
            {
                rootElement.RequestedTheme = AppTheme;
            }
        }
    }
}
