using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebUILayer.Services.Abstract;

namespace WebUILayer.Services.Concrete;

public class CookieAuthService : ICookieAuthService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CookieAuthService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task SignInWithJwtAsync(string accessToken, string? refreshToken = null, bool rememberMe = false)
    {
        var httpContext = _httpContextAccessor.HttpContext!;
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(accessToken);

        var identity = new ClaimsIdentity
            (
                jwt.Claims,
                CookieAuthenticationDefaults.AuthenticationScheme
            );

        await httpContext.SignInAsync
            (
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity),
                new AuthenticationProperties
                {
                    IsPersistent = rememberMe
                }
            );

        httpContext.Response.Cookies.Append("AccessToken", accessToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddHours(1)
        });

        if (!string.IsNullOrEmpty(refreshToken))
        {
            httpContext.Response.Cookies.Append("RefreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = rememberMe ? DateTimeOffset.UtcNow.AddDays(7) : DateTimeOffset.UtcNow.AddHours(1)
            });
        }
    }

    public async Task SignOutAsync()
    {
        var httpContext = _httpContextAccessor.HttpContext!;
        await httpContext.SignOutAsync();
        httpContext.Response.Cookies.Delete("AccessToken");
        httpContext.Response.Cookies.Delete("RefreshToken");
    }
}
