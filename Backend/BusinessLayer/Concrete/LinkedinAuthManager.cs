using Azure.Core;
using BusinessLayer.Abstract;
using DtoLayer.GuestBookDtos;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Net.Http.Headers;
using System.Text.Json;

namespace BusinessLayer.Concrete;

public class LinkedinAuthManager : ILinkedinAuthService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public LinkedinAuthManager(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task<OAuthUserProfileDto?> AuthenticateAsync(LinkedinAuthRequestDto linkedinAuthRequestDto, CancellationToken cancellationToken = default)
    {
        // Linkedin OAuth işlemi için gerekli olan clientId ve clientSecret değerlerini appsettings.json dosyasından alıyoruz.
        string clientId = _configuration["LinkedinOauth:ClientId"]!;
        string clientSecret = _configuration["LinkedinOauth:ClientSecret"]!;

        // HttpClientFactory kullanarak bir HttpClient örneği oluşturuyoruz. httpClientFactory, HttpClient örneklerini yönetmek ve yeniden kullanmak için kullanılan bir yapıdır. Bu sayede her istek için yeni bir HttpClient oluşturmak yerine, var olanları kullanarak performansı artırır ve kaynak kullanımını optimize eder.
        var client = _httpClientFactory.CreateClient();

        // Linkedin'den access token almak için gerekli olan HTTP isteğini hazırlıyoruz.
        using var tokenReq = new HttpRequestMessage(HttpMethod.Post, "https://www.linkedin.com/oauth/v2/accessToken");

        // İstek içeriği olarak form-url-encoded formatında gerekli parametreleri ekliyoruz.
        tokenReq.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            {
                "grant_type","authorization_code" // Linkedin OAuth için grant_type parametresi her zaman "authorization_code" olmalıdır.
            },
            {
                "code",linkedinAuthRequestDto.Code
                // Linkedin tarafından dönen authorization code, access token almak için kullanılır.
            },
            {
                "client_id",clientId
                // Linkedin OAuth uygulamanızın client ID'si, appsettings.json dosyasından alınır.
            },
            {
                "client_secret",clientSecret
                // Linkedin OAuth uygulamanızın client secret'ı, appsettings.json dosyasından alınır.
            },
            {
                "redirect_uri", linkedinAuthRequestDto.RedirectUri
                // Linkedin OAuth işlemi sırasında kullanılan redirect URI, appsettings.json dosyasından alınır.
            }
        });

        // Linkedin'e access token almak için HTTP isteğini gönderiyoruz.
        var tokenRes = await client.SendAsync(tokenReq,cancellationToken);

        // Eğer istek başarılı değilse, null döndürüyoruz.
        if (!tokenRes.IsSuccessStatusCode)
        {
            return null;
        }


        // İstek başarılı ise, response içeriğinden access token'ı alıyoruz.
        var tokenJson = await tokenRes.Content.ReadAsStringAsync(cancellationToken);
        // Access token'ı JSON formatında döndüğü için, JSON'u parse ederek access token'ı alıyoruz.
        using var tokenDoc = JsonDocument.Parse(tokenJson);
        // Eğer access token'ı JSON içinde bulamazsak, null döndürüyoruz.
        if (!tokenDoc.RootElement.TryGetProperty("access_token",out var accessTokenProp))
        {
            return null;
        }

        // Access token'ı aldıktan sonra, bu token'ı kullanarak Linkedin API'sinden kullanıcı bilgilerini almak için yeni bir HTTP isteği hazırlıyoruz.
        using var userReq = new HttpRequestMessage(HttpMethod.Get, "https://api.linkedin.com/v2/userinfo");
        // Authorization header'ına Bearer token olarak access token'ı ekliyoruz. Bu sayede Linkedin API'si, bu isteğin yetkilendirilmiş olduğunu anlayacak ve kullanıcı bilgilerini döndürecektir.
        userReq.Headers.Authorization = new AuthenticationHeaderValue("Bearer",accessTokenProp.GetString());

        // Linkedin API'sine kullanıcı bilgilerini almak için HTTP isteğini gönderiyoruz.
        var userRes = await client.SendAsync(userReq,cancellationToken);
        // Eğer istek başarılı değilse, null döndürüyoruz.
        if (!userRes.IsSuccessStatusCode)
        {
            return null;
        }
        // İstek başarılı ise, response içeriğinden kullanıcı bilgilerini alıyoruz.
        var userJson = await userRes.Content.ReadAsStringAsync(cancellationToken);
        // Kullanıcı bilgileri JSON formatında döndüğü için, JSON'u parse ederek kullanıcı bilgilerini alıyoruz.
        using var document = JsonDocument.Parse(userJson);
        // Kullanıcı bilgilerini JSON içinde bulamazsak, null döndürüyoruz.
        var root = document.RootElement;

        return new OAuthUserProfileDto
        {
            AuthProvider = "LinkedIn",
            AuthProviderId = root.GetProperty("sub").GetString() ?? "",
            AuthorName = root.GetProperty("name").GetString() ?? "Bilinmeyen",
            AuthorAvatarUrl = root.GetProperty("picture").GetString() ?? "",
            AuthorProfileUrl = ""
        };
    }
}
