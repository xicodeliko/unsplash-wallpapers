using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;

namespace UnsplashWallpapers.Services;

public class WallpaperSetter
{
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern int SystemParametersInfo(
        int uAction,
        int uParam,
        string lpvParam,
        int fuWinIni);

    private const int SPI_SETDESKWALLPAPER = 20;
    private const int SPIF_UPDATEINIFILE = 0x01;
    private const int SPIF_SENDCHANGE = 0x02;

    private readonly HttpClient _http = CreateHttpClient();

    private static HttpClient CreateHttpClient()
    {
        var client = new HttpClient();
        client.DefaultRequestHeaders.UserAgent.ParseAdd(
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) UnsplashWallpapersApp/1.0");
        return client;
    }

    // Descarga la imagen desde una URL y la guarda en una carpeta local.
    public async Task<string> DownloadImageAsync(string url, string fileName)
    {
        var folder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
            "UnsplashWallpapers");

        Directory.CreateDirectory(folder);

        var filePath = Path.Combine(folder, fileName + ".jpg");
        var bytes = await _http.GetByteArrayAsync(url);
        await File.WriteAllBytesAsync(filePath, bytes);

        return filePath;
    }

    // Aplica una imagen local como fondo de escritorio de Windows.
    public void SetWallpaper(string filePath)
    {
        SystemParametersInfo(
            SPI_SETDESKWALLPAPER,
            0,
            filePath,
            SPIF_UPDATEINIFILE | SPIF_SENDCHANGE);
    }
}
