using CV.EntityLayer.Entities;
using DtoLayer.GuestBookDtos;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebUILayer.Services.Abstract;

namespace WebUILayer.Controllers;

public class OAuthController : Controller
{
    private readonly IOAuthApiService _oAuthApiService;
    private readonly IConfiguration _configuration;

    public OAuthController(IOAuthApiService oAuthApiService, IConfiguration configuration)
    {
        _oAuthApiService = oAuthApiService;
        _configuration = configuration;
    }

    // GitHub OAuth Giriş
    [HttpGet]
    public IActionResult LoginGithub()
    {
        // GitHub OAuth yetkilendirme URL'sine yönlendirmek için gerekli parametreleri hazırla
        string clientId = _configuration["OAuth:GitHub:ClientId"]!;
        // GitHub OAuth yetkilendirme URL'sine yönlendir
        return Redirect($"https://github.com/login/oauth/authorize?client_id={clientId}");
    }


    // GitHub OAuth callback işlemi
    [HttpGet]
    public async Task<IActionResult> GithubCallback(string code)
    {
        // GitHub tarafından dönen kodu al ve API üzerinden kullanıcı bilgilerini al
        if (string.IsNullOrEmpty(code))
        {
            return RedirectToGuestBook("İptal Edildi");
        }
        // API üzerinden GitHub kullanıcı bilgilerini al
        var profile = await _oAuthApiService.GithubLoginAsync(new GithubAuthRequestDto
        {
            Code = code
            // RedirectUri genellikle GitHub OAuth için gerekli değildir, ancak API'niz bunu bekliyorsa ekleyebilirsiniz
        });

        return HandleOAuthProfile(profile);
    }



    // LinkedIn OAuth Giriş
    [HttpGet]
    public IActionResult LoginLinkedin()
    {
        // LinkedIn OAuth yetkilendirme URL'sine yönlendirmek için gerekli parametreleri hazırla
        string clientId = _configuration["OAuth:LinkedIn:ClientId"]!;
        // redirectUri, LinkedIn OAuth callback URL'si olarak API'nizde tanımladığınız URL ile aynı olmalıdır redirectUri nedir = API'nizde LinkedIn OAuth callback işlemi için tanımladığınız URL'dir. Genellikle, bu URL, API'nizin LinkedIn OAuth işlemlerini yönettiği bir endpoint'e işaret eder. Örneğin, API'nizde "/api/auth/linkedin/callback" gibi bir endpoint tanımladıysanız, redirectUri de bu URL'ye işaret etmelidir. Bu URL, LinkedIn tarafından kullanıcı yetkilendirme işlemi tamamlandıktan sonra geri çağrılacak ve API'nize kodu iletecektir. Bu nedenle, redirectUri'yi API'nizde tanımladığınız LinkedIn OAuth callback URL'si ile aynı yapmanız önemlidir.
        string redirectUri = Url.Action("LinkedinCallback", "OAuth", null, Request.Scheme)!;

        // LinkedIn OAuth yetkilendirme URL'sine yönlendir
        return Redirect($"https://www.linkedin.com/oauth/v2/authorization?response_type=code&client_id={clientId}&redirect_uri={redirectUri}&scope=openid%20profile%20email");
    }


    // LinkedIn OAuth callback işlemi
    [HttpGet]
    public async Task<IActionResult> LinkedinCallback(string code)
    {
        if (string.IsNullOrEmpty(code))
        {
            return RedirectToGuestBook("İptal Edildi");
        }

        // LinkedIn tarafından dönen kodu al ve API üzerinden kullanıcı bilgilerini al
        string redirectUri = Url.Action("LinkedinCallback", "OAuth", null, Request.Scheme)!;

        // API üzerinden LinkedIn kullanıcı bilgilerini al
        var profile = await _oAuthApiService.LinkedinLoginAsync(new LinkedinAuthRequestDto
        {
            Code = code,
            RedirectUri = redirectUri
        });

        return HandleOAuthProfile(profile);
    }


    // Ortak OAuth profil işleme metodu
    private IActionResult HandleOAuthProfile(OAuthUserProfileDto? oAuthUserProfileDto)
    {
        // API'den dönen kullanıcı bilgilerini kontrol et ve oturum aç
        if (oAuthUserProfileDto != null)
        {
            // Kullanıcı bilgilerini oturumda sakla (örneğin, Session veya TempData kullanarak)
            var guestUser = new CreateGuestBookDto
            {
                AuthProvider = oAuthUserProfileDto.AuthProvider,// API'den dönen OAuth sağlayıcı bilgilerini kullanarak oturum aç
                AuthProviderId = oAuthUserProfileDto.AuthProviderId,// API'den dönen OAuth sağlayıcı ID'sini kullanarak oturum aç
                AuthorName = oAuthUserProfileDto.AuthorName,// API'den dönen kullanıcı adını kullanarak oturum aç
                AuthorAvatarUrl = oAuthUserProfileDto.AuthorAvatarUrl,// API'den dönen kullanıcı avatar URL'sini kullanarak oturum aç
                AuthorProfileUrl = oAuthUserProfileDto.AuthorProfileUrl// API'den dönen kullanıcı profil URL'sini kullanarak oturum aç
            };
            // Oturumda kullanıcı bilgilerini sakla (örneğin, Session veya TempData kullanarak)
            HttpContext.Session.SetString("GuestUser", JsonSerializer.Serialize(guestUser));
            // Giriş başarılı, misafir defteri sayfasına yönlendir
            return RedirectToAction(nameof(GuestBookController.Index), GuestBookController.name);
        }
        return RedirectToGuestBook("Giriş Başarısız");
    }


    private IActionResult RedirectToGuestBook(string error)
    {
        TempData["ErrorMessage"] = error;
        return RedirectToAction(nameof(GuestBookController.Index), GuestBookController.name);
    }

}
