using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using UnsplashWallpapers.Models;

namespace UnsplashWallpapers.Services;

public class UnsplashService
{
    private readonly HttpClient _http;
    private const string BaseUrl = "https://api.unsplash.com";

    public UnsplashService(string accessKey)
    {
        _http = new HttpClient();
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Client-ID", accessKey);
    }

    // Busca fotos por palabra clave o topic (Unsplash usa el mismo endpoint 'search/photos' para ambos)
    public async Task<List<UnsplashPhoto>> SearchPhotosAsync(string query, int count = 10)
    {
        var randomPage = new Random().Next(1, 6); // página 1 a 5, para variar resultados
        var url = $"{BaseUrl}/search/photos?query={Uri.EscapeDataString(query)}&per_page={count}&page={randomPage}";
        var response = await _http.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        var results = doc.RootElement.GetProperty("results");
        var photos = new List<UnsplashPhoto>();
        foreach (var item in results.EnumerateArray())
        {
            photos.Add(new UnsplashPhoto
            {
                Id = item.GetProperty("id").GetString() ?? "",
                FullUrl = item.GetProperty("urls").GetProperty("full").GetString() ?? "",
                ThumbUrl = item.GetProperty("urls").GetProperty("thumb").GetString() ?? "",
                PhotographerName = item.GetProperty("user").GetProperty("name").GetString() ?? "",
                PhotographerLink = item.GetProperty("user").GetProperty("links").GetProperty("html").GetString() ?? "",
                PhotoLink = item.GetProperty("links").GetProperty("html").GetString() ?? ""
            });
        }

        return photos;
    }
}
