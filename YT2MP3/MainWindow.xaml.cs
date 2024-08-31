using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.UI.ApplicationSettings;
using System;
using Microsoft.UI;
using WinRT.Interop;
using Microsoft.UI.Windowing;
using Windows.Graphics;
using Windows.Storage;

namespace YT2MP3
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            ContentFrame.Navigate(typeof(DownloadPage));
            this.Activated += MainWindow_Activated;

            // Set window size
            this.Activate();
            var hwnd = WindowNative.GetWindowHandle(this);

            // Get the WindowId from the hwnd
            WindowId windowId = Win32Interop.GetWindowIdFromWindow(hwnd);

            // Get the AppWindow from the WindowId
            AppWindow appWindow = AppWindow.GetFromWindowId(windowId);

            // Resize the window
            appWindow.Resize(new SizeInt32(640, 480));

            CustomizeTitleBar();
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

        private void CustomizeTitleBar()
        {
            // Get the AppWindow
            var appWindow = GetAppWindowForCurrentWindow();

            if (appWindow != null)
            {
                // Enable Mica
                appWindow.TitleBar.ExtendsContentIntoTitleBar = true;

                // Set the title bar colors
                appWindow.TitleBar.ButtonBackgroundColor = Colors.Transparent;

                if ((int)SettingsManager.themeSetting == 0)  // Light theme
                {
                    appWindow.TitleBar.ButtonForegroundColor = Colors.Black;
                }
                else if ((int)SettingsManager.themeSetting == 1)  // Dark theme
                {
                    appWindow.TitleBar.ButtonForegroundColor = Colors.White;
                }

                appWindow.TitleBar.ButtonHoverBackgroundColor = Colors.LightGray;
                appWindow.TitleBar.ButtonHoverForegroundColor = Colors.Black;
                appWindow.TitleBar.ButtonPressedBackgroundColor = Colors.Gray;
                appWindow.TitleBar.ButtonPressedForegroundColor = Colors.White;
                appWindow.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
                appWindow.TitleBar.ButtonInactiveForegroundColor = Colors.Gray;

                // Set the window's background to Mica
                appWindow.TitleBar.BackgroundColor = Colors.Transparent;

            }
        }

        private AppWindow GetAppWindowForCurrentWindow()
        {
            IntPtr hWnd = WindowNative.GetWindowHandle(this);
            WindowId wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
            return AppWindow.GetFromWindowId(wndId);
        }
    }
}
