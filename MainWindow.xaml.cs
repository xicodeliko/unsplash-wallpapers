using System.Windows;
using System.Windows.Media.Imaging;
using UnsplashWallpapers.Models;
using UnsplashWallpapers.Services;

namespace UnsplashWallpapers;

public partial class MainWindow : Window
{
    private readonly UnsplashService _unsplashService;
    private readonly WallpaperSetter _wallpaperSetter = new();
    private List<UnsplashPhoto> _currentResults = new();
    private UnsplashPhoto? _currentPhoto;
    private System.Windows.Threading.DispatcherTimer? _autoTimer;
    private bool _isExiting = false;

    public MainWindow(string accessKey)
    {
        InitializeComponent();
        _unsplashService = new UnsplashService(accessKey);
        TrayIcon.IconSource = new BitmapImage(
            new Uri(System.IO.Path.Combine(AppContext.BaseDirectory, "Resources", "tray.ico")));

        Closing += (s, args) =>
        {
            if (!_isExiting)
            {
                args.Cancel = true;
                Hide();
            }
        };
    }

    private string GetQuery()
    {
        if (!string.IsNullOrWhiteSpace(KeywordTextBox.Text))
        {
            return KeywordTextBox.Text.Trim();
        }

        if (TopicComboBox.SelectedItem is System.Windows.Controls.ComboBoxItem item)
        {
            return item.Content.ToString() ?? "nature";
        }

        return "nature";
    }

    private async void SearchButton_Click(object sender, RoutedEventArgs e)
    {
        var query = GetQuery();
        _currentResults = await _unsplashService.SearchPhotosAsync(query, 10);

        if (_currentResults.Count == 0)
        {
            MessageBox.Show("No se encontraron fotos para esa búsqueda.");
            return;
        }

        _currentPhoto = _currentResults[0];
        PreviewImage.Source = new BitmapImage(new Uri(_currentPhoto.ThumbUrl));
        PhotographerText.Text = $"Foto de {_currentPhoto.PhotographerName}";
    }

    private void TrayIcon_TrayLeftMouseUp(object sender, RoutedEventArgs e)
    {
        Show();
        WindowState = WindowState.Normal;
        Activate();
    }

    private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
    {
        Show();
        WindowState = WindowState.Normal;
        Activate();
    }

    private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
    {
        _isExiting = true;
        TrayIcon.Dispose();
        System.Windows.Application.Current.Shutdown();
    }

    private async void ApplyButton_Click(object sender, RoutedEventArgs e)
    {
        if (_currentPhoto == null)
        {
            MessageBox.Show("Primero busca una foto.");
            return;
        }

        var path = await _wallpaperSetter.DownloadImageAsync(_currentPhoto.FullUrl, _currentPhoto.Id);
        _wallpaperSetter.SetWallpaper(path);
    }

    private async void DownloadButton_Click(object sender, RoutedEventArgs e)
    {
        if (_currentPhoto == null)
        {
            MessageBox.Show("Primero busca una foto.");
            return;
        }

        var path = await _wallpaperSetter.DownloadImageAsync(_currentPhoto.FullUrl, _currentPhoto.Id);
        MessageBox.Show($"Imagen guardada en: {path}");
    }

    private int GetIntervalMinutes()
    {
        if (IntervalComboBox.SelectedItem is System.Windows.Controls.ComboBoxItem item)
        {
            var tag = item.Tag?.ToString();

            if (tag == "custom" && int.TryParse(CustomMinutesTextBox.Text, out var customMin))
            {
                return customMin;
            }

            if (tag != null && tag != "custom" && int.TryParse(tag, out var min))
            {
                return min;
            }
        }

        return 60;
    }

    private async void AutoChangeButton_Click(object sender, RoutedEventArgs e)
    {
        if (_autoTimer != null)
        {
            _autoTimer.Stop();
            _autoTimer = null;
            AutoChangeButton.Content = "Iniciar cambio automático";
            return;
        }

        var minutes = GetIntervalMinutes();
        _autoTimer = new System.Windows.Threading.DispatcherTimer
        {
            Interval = TimeSpan.FromMinutes(minutes)
        };

        _autoTimer.Tick += async (s, args) => await ChangeWallpaperRandomly();
        _autoTimer.Start();
        AutoChangeButton.Content = "Detener cambio automático";

        await ChangeWallpaperRandomly();
    }

    private async Task ChangeWallpaperRandomly()
    {
        try
        {
            // Si no hay fotos cargadas todavía, busca una vez.
            // Si ya hay resultados de una búsqueda previa, los reutiliza
            // en vez de golpear la API cada vez (evita el límite de 50/hora).
            if (_currentResults.Count == 0)
            {
                var query = GetQuery();
                _currentResults = await _unsplashService.SearchPhotosAsync(query, 10);
            }
            if (_currentResults.Count == 0) return;
            var random = new Random();
            _currentPhoto = _currentResults[random.Next(_currentResults.Count)];
            var path = await _wallpaperSetter.DownloadImageAsync(_currentPhoto.FullUrl, _currentPhoto.Id);
            _wallpaperSetter.SetWallpaper(path);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"No se pudo cambiar el fondo automáticamente:\n\n{ex.Message}");
        }
    }
}
