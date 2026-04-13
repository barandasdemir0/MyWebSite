using DtoLayer.AuthDtos.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using SharedKernel.Enums;
using WebUILayer.Services.Abstract;

namespace WebUILayer.Controllers
{
    public class TwoFactorController : Controller
    {
       
        private readonly ITwoFactorApiService _twoFactorApiService;
        private readonly ICookieAuthService _cookieAuthService;
        public const string Name = "TwoFactor"; // Sabit isim

        public TwoFactorController(ITwoFactorApiService twoFactorApiService, ICookieAuthService cookieAuthService)
        {
            _twoFactorApiService = twoFactorApiService;
            _cookieAuthService = cookieAuthService;
        }

      

     

        [HttpGet("/auth/choose-2fa")]
        public IActionResult ChooseTwoFactor() => View();


        //bu method kullanıcı 2FA yöntemini seçtikten sonra çalışacak. Seçilen yönteme göre kullanıcıyı ilgili doğrulama sayfasına yönlendirecek.
        [HttpPost("/auth/process-2fa-choice")]
        public async Task<IActionResult> ProcessTwoFactorChoice(TwoFactorProvider SelectedProvider)
        {
            var userId = HttpContext.Session.GetString("2FA_UserId"); // Kullanıcı ID'sini oturumdan al biz 2FA_UserId bilgisini Login işleminde oturuma kaydediyoruz
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction(nameof(AuthController.Login),AuthController.Name);
            }
            if (SelectedProvider == TwoFactorProvider.Email)
            {
                return RedirectToAction(nameof(SendEmailCode));
            }
            HttpContext.Session.SetString("2FA_Provider", SelectedProvider.ToString()); // Seçilen 2FA yöntemini oturuma kaydet
            return RedirectToAction(nameof(VerifyTwoFactor));
        }




        [HttpGet("/auth/send-email-code")]
        public async Task<IActionResult> SendEmailCode()
        {
            var userId = HttpContext.Session.GetString("2FA_UserId"); // Kullanıcı ID'sini oturumdan al
            if (string.IsNullOrEmpty(userId)) return RedirectToAction(nameof(AuthController.Login), AuthController.Name);
            await _twoFactorApiService.SendEmailCodeAsync(userId!); // Email ile doğrulama kodunu gönder
            HttpContext.Session.SetString("2FA_Provider", TwoFactorProvider.Email.ToString()); 
            return RedirectToAction(nameof(VerifyTwoFactor));

        }

        [HttpGet("/auth/verify-2fa")]
        public IActionResult VerifyTwoFactor()
        {
            var provider = HttpContext.Session.GetString("2FA_Provider") ?? TwoFactorProvider.Email.ToString();
            ViewBag.provider = provider;
            return View();
        }

        [HttpPost("/auth/verify-2fa")]
        public async Task<IActionResult> VerifyTwoFactor(string code)
        {
            var userId = HttpContext.Session.GetString("2FA_UserId"); // Kullanıcı ID'sini oturumdan al
            var provider = HttpContext.Session.GetString("2FA_Provider") ?? TwoFactorProvider.Email.ToString();// Seçilen 2FA yöntemini oturumdan al, eğer yoksa varsayılan olarak Email kullan

            var result = await _twoFactorApiService.VerifyTwoFactorAsync // 2FA doğrulamasını API üzerinden gerçekleştir
                (
                new TwoFactorVerifyDto
                {
                    UserId = userId!, // Kullanıcı ID'si
                    Code = code, // Kullanıcının girdiği doğrulama kodu
                    Provider = Enum.Parse<TwoFactorProvider>(provider), // Seçilen 2FA sağlayıcısı (Email, Authenticator vb.)
                    DeviceInfo = Request.Headers["User-Agent"].ToString() // İsteği yapan cihaz bilgisi (isteğe bağlı, güvenlik için kullanılabilir)
                }
                );
            if (result == null || !result.Success)
            {
                ModelState.AddModelError("", result?.Error ?? "Geçersiz Kod");
                return View();
            }
            await _cookieAuthService.SignInWithJwtAsync(result.Token!, result.RefreshToken); // JWT token'ı kullanarak kullanıcıyı oturum açtır
            HttpContext.Session.Clear(); // 2FA ile ilgili oturum bilgilerini temizle (Kullanıcı ID'si, seçilen 2FA yöntemi vb.)
            return RedirectToAction("Index", "Dashboard", new { area = "Admin" }); // Doğrulama başarılı ise kullanıcıyı dashboard'a yönlendir
        }
    }
}
