using System.Windows;
using UnsplashWallpapers.Services;

namespace UnsplashWallpapers;

public partial class App : System.Windows.Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        DispatcherUnhandledException += (s, args) =>
        {
            MessageBox.Show($"Error no manejado:\n\n{args.Exception}");
            args.Handled = true;
        };

        var settingsService = new SettingsService();
        var settings = settingsService.Load();
        var accessKey = settings.UnsplashAccessKey;

        if (string.IsNullOrWhiteSpace(accessKey))
        {
            var setupWindow = new ApiKeySetupWindow(settingsService);
            var result = setupWindow.ShowDialog();
            if (result != true)
            {
                Shutdown();
                return;
            }

            accessKey = setupWindow.SavedAccessKey;
        }

        var mainWindow = new MainWindow(accessKey!);
        mainWindow.Show();
    }
}
