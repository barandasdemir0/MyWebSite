using BusinessLayer.Abstract;
using DtoLayer.GuestBookDtos;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text.Json;

namespace BusinessLayer.Concrete;

public class GithubAuthManager : IGithubAuthService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public GithubAuthManager(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task<OAuthUserProfileDto?> AuthenticateAsync(GithubAuthRequestDto githubAuthRequestDto,CancellationToken cancellationToken=default)
    {
        // GitHub OAuth için Client ID ve Client Secret'ı yapılandırmadan al
        string clientId = _configuration["GithubOauth:ClientId"]!;
        string clientSecret = _configuration["GithubOauth:ClientSecret"]!;

        // GitHub API'sine istek göndermek için HttpClient oluştur
        var client = _httpClientFactory.CreateClient("GithubApi");


        // Access token almak için GitHub'e POST isteği gönder
        var tokenReq = new HttpRequestMessage(HttpMethod.Post, $"https://github.com/login/oauth/access_token?client_id={clientId}&client_secret={clientSecret}&code={githubAuthRequestDto.Code}");

        // GitHub API'si JSON formatında yanıt döndüreceği için Accept header'ını ekle
        tokenReq.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        // Access token alma isteğini gönder
        var tokenRes = await client.SendAsync(tokenReq,cancellationToken);
        if (!tokenRes.IsSuccessStatusCode)
        {
            return null;
        }

        // Access token'ı yanıtın JSON içeriğinden çıkar
        var tokenJson = await tokenRes.Content.ReadAsStringAsync(cancellationToken);
        // GitHub'in access token'ı JSON formatında döndürdüğü varsayılarak, JSON'u ayrıştır
        using var tokenDoc = JsonDocument.Parse(tokenJson);

        // Access token'ı JSON içinden çıkar
        if (!tokenDoc.RootElement.TryGetProperty("access_token",out var accessTokenProp))
        {
            return null;
        }

        // Access token'ı kullanarak GitHub API'sinden kullanıcı bilgilerini al
        var userReq = new HttpRequestMessage(HttpMethod.Get, "user");

        // GitHub API'sine erişim için Authorization header'ına Bearer token ekle
        userReq.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessTokenProp.GetString()!);

        // Kullanıcı bilgilerini alma isteğini gönder
        var userRes = await client.SendAsync(userReq,cancellationToken);
        if (!userRes.IsSuccessStatusCode)
        {
            return null;
        }

        // Kullanıcı bilgilerini yanıtın JSON içeriğinden çıkar
        var userJson = await userRes.Content.ReadAsStringAsync(cancellationToken);
        // GitHub'in kullanıcı bilgilerini JSON formatında döndürdüğü varsayılarak, JSON'u ayrıştır
        using var document = JsonDocument.Parse(userJson);
        // JSON içinden gerekli kullanıcı bilgilerini çıkararak OAuthUserProfileDto oluştur
        var root = document.RootElement;


        // GitHub API'sinden dönen JSON'da "id", "login", "avatar_url" ve "html_url" gibi alanların olduğunu varsayıyoruz
        return new OAuthUserProfileDto
        {
            AuthProvider = "GitHub",
            AuthProviderId = root.GetProperty("id").ToString(),
            AuthorName = root.GetProperty("login").GetString() ?? "Bilinmeyen",
            AuthorAvatarUrl = root.GetProperty("avatar_url").GetString() ?? "",
            AuthorProfileUrl = root.GetProperty("html_url").GetString() ?? ""

        };
    }
}
