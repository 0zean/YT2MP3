using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.UI.ApplicationSettings;
using System;


namespace YT2MP3
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            SettingsManager.ApplyTheme(this);
            ContentFrame.Navigate(typeof(DownloadPage));
            this.Activated += MainWindow_Activated;
        }

        private void MainWindow_Activated(object sender, WindowActivatedEventArgs args)
        {
            if (args.WindowActivationState != WindowActivationState.Deactivated)
            {
                this.Activated -= MainWindow_Activated;
                CheckDependencies();
            }
        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItemContainer != null)
            {
                var navItemTag = args.SelectedItemContainer.Tag.ToString();
                NavView_Navigate(navItemTag);
            }
        }

        private async void CheckDependencies()
        {
            if (Content?.XamlRoot != null)
            {
                bool dependenciesInstalled = await DependencyChecker.CheckAndInstallDependencies(Content.XamlRoot);
                if (!dependenciesInstalled)
                {
                    ContentDialog warningDialog = new ContentDialog
                    {
                        Title = "Warning",
                        Content = "Some dependencies are missing. The application may not function correctly.",
                        CloseButtonText = "OK",
                        XamlRoot = Content.XamlRoot
                    };

                    await warningDialog.ShowAsync();
                }
            }
        }

        private void NavView_Navigate(string navItemTag)
        {
            Type _page = null;
            switch (navItemTag)
            {
                case "download":
                    _page = typeof(DownloadPage);
                    break;
                case "settings":
                    _page = typeof(SettingsPage);
                    break;
            }
            if (_page != null && ContentFrame.CurrentSourcePageType != _page)
            {
                ContentFrame.Navigate(_page);
            }
        }
    }
}
