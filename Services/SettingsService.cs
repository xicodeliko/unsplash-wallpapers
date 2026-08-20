using System.IO;
using System.Text.Json;
using UnsplashWallpapers.Models;

namespace UnsplashWallpapers.Services;

public class SettingsService
{
    private readonly string _filePath;

    public SettingsService()
    {
        var folder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "UnsplashWallpapers");

        Directory.CreateDirectory(folder);
        _filePath = Path.Combine(folder, "settings.json");
    }

    public AppSettings Load()
    {
        if (!File.Exists(_filePath))
            return new AppSettings();

        var json = File.ReadAllText(_filePath);
        return JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
    }

    public void Save(AppSettings settings)
    {
        var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }
}
