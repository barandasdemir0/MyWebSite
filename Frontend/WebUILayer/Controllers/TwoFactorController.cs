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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessTwoFactorChoice(TwoFactorProvider SelectedProvider)
        {
            var userId = HttpContext.Session.GetString("2FA_UserId"); // Kullanıcı ID'sini oturumdan al biz 2FA_UserId bilgisini Login işleminde oturuma kaydediyoruz
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction(nameof(AuthController.Login), AuthController.Name);
            }
            if (SelectedProvider == TwoFactorProvider.Email)
            {
                try
                {
                    // Yönlendirme (Redirect) yok! E-postayı direkt burada atıyoruz.
                    await _twoFactorApiService.SendEmailCodeAsync(userId);
                }
                catch (Exception) { /* Arka planda loglanabilir */ }
            }
            HttpContext.Session.SetString("2FA_Provider", SelectedProvider.ToString());
            return RedirectToAction(nameof(VerifyTwoFactor)); // E-posta atıldıktan sonra doğrulama ekranına geç
        }




        [HttpPost("/auth/send-email-code")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendEmailCode()
        {
            var userId = HttpContext.Session.GetString("2FA_UserId"); // Kullanıcı ID'sini oturumdan al
            if (string.IsNullOrEmpty(userId)) return RedirectToAction(nameof(AuthController.Login), AuthController.Name);
            try
            {
                await _twoFactorApiService.SendEmailCodeAsync(userId!); // Email ile doğrulama kodunu gönder
                HttpContext.Session.SetString("2FA_Provider", TwoFactorProvider.Email.ToString());
                TempData["SuccessMessage"] = "Doğrulama kodu e-posta adresinize tekrar gönderildi.";
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "E-posta gönderilirken bir hata oluştu.";
            }
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyTwoFactor(string code)
        {
            var userId = HttpContext.Session.GetString("2FA_UserId");
            var provider = HttpContext.Session.GetString("2FA_Provider") ?? TwoFactorProvider.Email.ToString();
            if (string.IsNullOrWhiteSpace(code))
            {
                ModelState.AddModelError("", "Lütfen geçerli bir kod girin.");
                ViewBag.provider = provider;
                return View();
            }
            try
            {
                var result = await _twoFactorApiService.VerifyTwoFactorAsync(
                    new TwoFactorVerifyDto
                    {
                        UserId = userId!,
                        Code = code,
                        Provider = Enum.Parse<TwoFactorProvider>(provider),
                        DeviceInfo = Request.Headers["User-Agent"].ToString()
                    });
                if (result == null || !result.Success)
                {
                    ModelState.AddModelError("", result?.Error ?? "Geçersiz Kod");
                    ViewBag.provider = provider;
                    return View();
                }
                await _cookieAuthService.SignInWithJwtAsync(result.Token!, result.RefreshToken);
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Doğrulama sırasında sistemsel bir hata oluştu.");
                ViewBag.provider = provider;
                return View();
            }
        }
    }
}
