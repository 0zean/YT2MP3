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

        public static int themeSetting
        {
            get => ((int?)localSettings.Values["themeSetting"] ?? 1);
            set => localSettings.Values["themeSetting"] = (int)value;
        }
    }
}
