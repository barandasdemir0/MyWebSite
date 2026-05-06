using DtoLayer.AuthDtos.Requests;
using Microsoft.AspNetCore.Mvc;
using WebUILayer.Services.Abstract;

namespace WebUILayer.Controllers;

public class AuthController : Controller
{
    private readonly IAuthApiService _authApiService;
    private readonly ICookieAuthService _cookieAuthService;
    public const string Name = "Auth"; // Sabit isim
    public AuthController(IAuthApiService authApiService, ICookieAuthService cookieAuthService)
    {
        _authApiService = authApiService;
        _cookieAuthService = cookieAuthService;
    }

    //login ve register işlemleri için ayrı ayrı actionlar oluşturduk. Login işleminde, kullanıcıdan gelen bilgileri API'ye gönderiyoruz ve başarılı olursa JWT token'ı alıp cookie'ye kaydediyoruz. Register işleminde ise kullanıcıyı API'ye kaydediyoruz ve başarılı olursa login sayfasına yönlendiriyoruz.
    [HttpGet("/auth/login")]
    public IActionResult Login() => View();

    [HttpPost("/auth/login")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        if (!ModelState.IsValid)
        {
            return View(loginDto);
        }
        try
        {
            loginDto.DeviceInfo = Request.Headers["User-Agent"].ToString();//sadece bu satırı 5 kelime ile anlat : Kullanıcının tarayıcı bilgilerini alır. user-agent nedir : User-Agent, tarayıcı tanımlayıcısıdır. 
            var result = await _authApiService.LoginAsync(loginDto); // API'ye login bilgilerini gönderir ve sonucu alır.
            if (result == null || !result.Success) // Eğer sonuç başarısızsa, hata mesajını model durumuna ekler ve login sayfasını tekrar gösterir.
            {
                ModelState.AddModelError("", result?.Error ?? "Bağlantı Hatası");
                return View(loginDto);
            }
            if (result.RequiresTwoFactor) // Eğer iki faktörlü doğrulama gerekiyorsa, kullanıcı ID'sini session'a kaydeder ve iki faktör seçme sayfasına yönlendirir.
            {
                HttpContext.Session.SetString("2FA_UserId", result.UserId!);  // kısaca kullanıcının ID'sini session'a kaydeder. Bu ID, iki faktörlü doğrulama sürecinde kullanılacaktır.
                return RedirectToAction(nameof(TwoFactorController.ChooseTwoFactor), TwoFactorController.Name); // İki faktörlü doğrulama seçme sayfasına yönlendirir.
            }
            await _cookieAuthService.SignInWithJwtAsync(result.Token!, result.RefreshToken, loginDto.RememberMe); // Eğer iki faktörlü doğrulama gerekmiyorsa, JWT token'ını cookie'ye kaydeder ve kullanıcıyı admin paneline yönlendirir.
            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
        }
        catch (Exception ex)
        {
            // Sunucu/API çökerse beyaz 500 hatası yerine, kullanıcıya kırmızı uyarı gösterir
            ModelState.AddModelError("", "Sisteme giriş yapılırken bir hata oluştu: " + ex.Message);
            return View(loginDto);
        }
    }




    [HttpGet("/auth/register")]
    public IActionResult Register() => View();

    [HttpPost("/auth/register")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        // Validasyon kontrolü eklendi
        if (!ModelState.IsValid)
        {
            return View(registerDto);
        }
        try
        {
            var result = await _authApiService.RegisterAsync(registerDto);
            if (result == null || !result.Success)
            {
                foreach (var error in result?.Errors ?? []) // Eğer kayıt başarısızsa, hata mesajlarını model durumuna ekler ve kayıt sayfasını tekrar gösterir. [] ifadesi, result.Errors null ise boş bir liste döndürür, böylece foreach döngüsü hata vermez.
                {
                    ModelState.AddModelError("", error);
                }
                return View(registerDto);
            }
            TempData["Success"] = "Kayıt başarılı, giriş yapabilirsiniz.";
            return RedirectToAction(nameof(Login));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "Kayıt işlemi sırasında bir sunucu hatası oluştu: " + ex.Message);
            return View(registerDto);
        }
    }


    [HttpPost("/auth/logout")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        try
        {
            await _authApiService.LogoutAsync();
            await _cookieAuthService.SignOutAsync();
        }
        catch (Exception)
        {
            // API hata verse bile kullanıcının tarayıcı Cookie'sini silerek güvenli çıkışı garantileriz
            await _cookieAuthService.SignOutAsync();
        }
        return RedirectToAction(nameof(Login)); // Admin login'e dönsün

    }





}
