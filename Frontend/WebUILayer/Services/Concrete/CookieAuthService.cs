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


    // JWT token'ını okuyarak kullanıcı bilgilerini çıkarır, cookie'ye kaydeder ve istenirse uzun süreli oturum sağlar.
    public async Task SignInWithJwtAsync(string accessToken, string? refreshToken = null, bool rememberMe = false)
    {
       
        var httpContext = _httpContextAccessor.HttpContext!;//HTTP bağlamına erişir. HTTP bağlamı, istek ve yanıt bilgilerini içerir.
        var handler = new JwtSecurityTokenHandler(); // JWT token'ını işlemek için bir JwtSecurityTokenHandler oluşturur.
        var jwt = handler.ReadJwtToken(accessToken); // JWT token'ını okuyarak içindeki bilgileri çıkarır. Bu bilgiler, kullanıcının kimliği ve yetkileri gibi bilgileri içerir.

        var identity = new ClaimsIdentity
            (
                jwt.Claims, // JWT token'ından çıkarılan bilgileri kullanarak bir ClaimsIdentity oluşturur. ClaimsIdentity, kullanıcının kimliği ve yetkilerini temsil eder.
                CookieAuthenticationDefaults.AuthenticationScheme // AuthenticationScheme, cookie tabanlı kimlik doğrulama için kullanılan varsayılan şemadır. Bu, cookie'lerin nasıl yönetileceğini belirler.
            );

        await httpContext.SignInAsync // Kullanıcıyı cookie'ye kaydeder ve oturum açar. Bu, kullanıcının kimliğini doğrulamak ve yetkilerini yönetmek için kullanılır.
            (
                CookieAuthenticationDefaults.AuthenticationScheme, // AuthenticationScheme, cookie tabanlı kimlik doğrulama için kullanılan varsayılan şemadır. Bu, cookie'lerin nasıl yönetileceğini belirler.
                new ClaimsPrincipal(identity), // ClaimsPrincipal, kullanıcının kimliğini ve yetkilerini temsil eder. Bu, kimlik doğrulama sürecinde kullanılır.
                new AuthenticationProperties // AuthenticationProperties, oturumun nasıl yönetileceğini belirler. Örneğin, IsPersistent özelliği, oturumun tarayıcı kapatılsa bile devam edip etmeyeceğini belirler.
                {
                    IsPersistent = rememberMe // rememberMe parametresi, kullanıcının oturumunun tarayıcı kapatılsa bile devam edip etmeyeceğini belirler. Eğer rememberMe true ise, oturum kalıcı olur ve tarayıcı kapatılsa bile devam eder. Eğer false ise, oturum geçici olur ve tarayıcı kapatıldığında sona erer.
                }
            );

        httpContext.Response.Cookies.Append("AccessToken", accessToken, new CookieOptions // AccessToken'ı cookie'ye kaydeder. Bu, kullanıcının kimliğini doğrulamak ve yetkilerini yönetmek için kullanılır.
        {
            HttpOnly = true, // HttpOnly özelliği, cookie'nin JavaScript tarafından erişilememesini sağlar. Bu, XSS saldırılarına karşı koruma sağlar.
            Secure = true, // Secure özelliği, cookie'nin sadece HTTPS üzerinden gönderilmesini sağlar. Bu, cookie'nin güvenliğini artırır.
            SameSite = SameSiteMode.Strict, // SameSite özelliği, cookie'nin hangi durumlarda gönderileceğini belirler. Strict modu, cookie'nin sadece aynı site içindeki isteklerde gönderilmesini sağlar. Bu, CSRF saldırılarına karşı koruma sağlar.
            Expires = DateTimeOffset.UtcNow.AddHours(1) // Expires özelliği, cookie'nin ne zaman sona ereceğini belirler. Bu örnekte, cookie 1 saat sonra sona erecektir. Bu, güvenliği artırır ve gereksiz uzun süreli oturumları önler.
        });

        if (!string.IsNullOrEmpty(refreshToken))
        {
            httpContext.Response.Cookies.Append("RefreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,//
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = rememberMe ? DateTimeOffset.UtcNow.AddDays(7) : DateTimeOffset.UtcNow.AddHours(1)
                // RefreshToken, accessToken'dan daha uzun süre geçerli olabilir. Eğer rememberMe true ise, refreshToken 7
            });
        }
    }

    public async Task SignOutAsync()
    {
        var httpContext = _httpContextAccessor.HttpContext!;//HTTP bağlamına erişir. HTTP bağlamı, istek ve yanıt bilgilerini içerir.
        await httpContext.SignOutAsync();// Kullanıcıyı cookie'den çıkarır ve oturumu kapatır. Bu, kullanıcının kimliğini doğrulamak ve yetkilerini yönetmek için kullanılır.
        httpContext.Response.Cookies.Delete("AccessToken"); // AccessToken'ı cookie'den siler. Bu, kullanıcının kimliğini doğrulamak ve yetkilerini yönetmek için kullanılır.
        httpContext.Response.Cookies.Delete("RefreshToken"); // RefreshToken'ı cookie'den siler. Bu, kullanıcının kimliğini doğrulamak ve yetkilerini yönetmek için kullanılır.
    }
}
