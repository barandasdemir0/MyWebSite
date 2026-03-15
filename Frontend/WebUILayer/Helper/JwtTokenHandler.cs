using DtoLayer.AuthDtos;
using System.Configuration;
using System.Net.Http.Headers;

namespace WebUILayer.Helper;

public class JwtTokenHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;

    public JwtTokenHandler(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
    {
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
    }


    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = _httpContextAccessor.HttpContext?.Request.Cookies["AccessToken"];
        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            var refreshToken = _httpContextAccessor.HttpContext?.Request.Cookies["RefreshToken"];
            if (!string.IsNullOrEmpty(refreshToken) && !string.IsNullOrEmpty(token))
            {
                var newToken = await TryRefreshAsync(token, refreshToken, cancellationToken);
                if (newToken!=null)
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newToken);
                    response = await base.SendAsync(request, cancellationToken);
                }
            }
        }
        return response;
    }


    private async Task<string?> TryRefreshAsync(string accessToken,string refreshToken,CancellationToken cancellationToken)
    {
        var baseUrl = _configuration["ApiSettings:Baseurl"];
        using var http = new HttpClient
        {
            BaseAddress = new Uri(baseUrl!)
        };

        var dto = new RefreshTokenRequestDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };

        var response = await http.PostAsJsonAsync("auth/refresh-token", dto, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var result = await response.Content.ReadFromJsonAsync<LoginResultDto>(cancellationToken: cancellationToken);
        if (result?.Token == null)
        {
            return null;
        }

        var ctx = _httpContextAccessor.HttpContext;
        var cookieOpt = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict
        };
        ctx?.Response.Cookies.Append("AccessToken", result.Token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.Now.AddDays(7)
        });

        return result.Token;
    }

}
