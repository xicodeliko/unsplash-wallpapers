using System.Windows;
using UnsplashWallpapers.Models;
using UnsplashWallpapers.Services;

namespace UnsplashWallpapers;

public partial class ApiKeySetupWindow : Window
{
    private readonly SettingsService _settingsService;

    public string? SavedAccessKey { get; private set; }

    public ApiKeySetupWindow(SettingsService settingsService)
    {
        InitializeComponent();
        _settingsService = settingsService;
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        var key = AccessKeyTextBox.Text.Trim();

        if (string.IsNullOrWhiteSpace(key))
        {
            MessageBox.Show("Ingresa una Access Key válida.");
            return;
        }

        _settingsService.Save(new AppSettings { UnsplashAccessKey = key });
        SavedAccessKey = key;
        DialogResult = true;
        Close();
    }
}
