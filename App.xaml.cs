using System.Windows;
using Microsoft.Extensions.Configuration;

namespace UnsplashWallpapers;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var config = new ConfigurationBuilder()
            .AddUserSecrets<App>()
            .Build();

        var accessKey = config["Unsplash:AccessKey"];

        if (string.IsNullOrWhiteSpace(accessKey))
        {
            MessageBox.Show("No se encontró la Access Key de Unsplash en user-secrets.");
            Shutdown();
            return;
        }

        var mainWindow = new MainWindow(accessKey);
        mainWindow.Show();
    }
}
