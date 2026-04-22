using DtoLayer.AuthDtos.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using SharedKernel.Enums;
using WebUILayer.Services.Abstract;

namespace WebUILayer.Controllers
{
    public class AccountController : Controller
    {

        private readonly IAccountApiService _accountApiService;
        public AccountController(IAccountApiService accountApiService)
        {
            _accountApiService = accountApiService;
        }

        [HttpGet("/account/forgot-password")]
        public IActionResult ForgotPassword() => View();

        [HttpPost("/account/forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
        {
            // Not: Burada API çağrısı yapılmıyor çünkü kullanıcı önce doğrulama yöntemini
            // seçecek (Email/Authenticator). OTP gönderimi ProcessResetChoice'da yapılıyor.

            HttpContext.Session.SetString("reset_email", forgotPasswordDto.Email); // Email bilgisini session'a kaydediyoruz, böylece sonraki adımlarda kullanabiliriz.
            return RedirectToAction(nameof(ChooseResetMethod));
        }

        [HttpGet("/account/ChooseResetMethod")]
        public IActionResult ChooseResetMethod()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("reset_email"))) // Güvenlik için, eğer email bilgisi yoksa doğrudan login sayfasına yönlendiriyoruz.
            {
                return RedirectToAction(nameof(AuthController.Login), AuthController.Name);
            }
            return View();
        }

        [HttpPost("/account/process-reset-choice")]
        public async Task<IActionResult> ProcessResetChoice(TwoFactorProvider SelectedProvider)
        {
            var email = HttpContext.Session.GetString("reset_email");  // Session'dan email bilgisini alıyoruz.
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction(nameof(AuthController.Login), AuthController.Name);// Güvenlik için, eğer email bilgisi yoksa doğrudan login sayfasına yönlendiriyoruz.
            }
            if (SelectedProvider == TwoFactorProvider.Email) // Eğer kullanıcı email seçtiyse, API'ye forgot password isteği gönderiyoruz. Authenticator seçilirse, OTP zaten Authenticator uygulamasında oluşturulacağı için API çağrısına gerek yok. çünkü sürekli olarak OTP oluşturuluyor ve doğrulanıyor olacak.
            {
                await _accountApiService.ForgotPasswordAsync(new ForgotPasswordDto
                {
                    Email = email
                });
            }

            HttpContext.Session.SetString("reset_provider", SelectedProvider.ToString()); // Seçilen doğrulama yöntemini session'a kaydediyoruz, böylece sonraki adımlarda hangi yöntemin seçildiğini bilebiliriz.
            return RedirectToAction(nameof(VerifyResetCode));
        }


        [HttpGet("/account/resend-reset-code")]
        public async Task<IActionResult> ResendResetCode()
        {
            var email = HttpContext.Session.GetString("reset_email"); // Session'dan email bilgisini alıyoruz.
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction(nameof(AuthController.Login), AuthController.Name); // Güvenlik için, eğer email bilgisi yoksa doğrudan login sayfasına yönlendiriyoruz.
            }
           
            await _accountApiService.ForgotPasswordAsync(new ForgotPasswordDto // Kodun tekrar gönderilmesi için API'ye forgot password isteği gönderiyoruz.
            {
                Email = email 
            });
            TempData["Info"] = "Kod tekrar gönderildi.";
            return RedirectToAction(nameof(VerifyResetCode));
        }




        [HttpGet("/account/verify-reset-code")]
        public IActionResult VerifyResetCode()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("reset_email"))) // Güvenlik için, eğer email bilgisi yoksa doğrudan login sayfasına yönlendiriyoruz.
            {
                return RedirectToAction(nameof(AuthController.Login), AuthController.Name);
            }
            return View();
        }

        [HttpPost("/account/verify-reset-code")]
        public async Task<IActionResult> VerifyResetCode(string code)
        {
            var email = HttpContext.Session.GetString("reset_email"); // Session'dan email bilgisini alıyoruz.
            var providerStr = HttpContext.Session.GetString("reset_provider") ?? TwoFactorProvider.Email.ToString();// Session'dan seçilen doğrulama yöntemini alıyoruz, eğer yoksa varsayılan olarak Email kabul ediyoruz.



            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction(nameof(AuthController.Login), AuthController.Name);
            }
            var provider = Enum.Parse<TwoFactorProvider>(providerStr); // String olarak kaydedilen provider bilgisini enum tipine dönüştürüyoruz. TwoFactorProvider.Email.ToString(); buradaki nasıl enumu strine dönüştürdüysek, stringi de enum'a dönüştürmemiz gerekiyor.
            var resetToken = await _accountApiService.VerifyResetOtpAsync(new VerifyResetOtpDto // API'ye doğrulama kodunu doğrulaması için istek gönderiyoruz.
            {
                Email = email,
                Provider = provider, // Seçilen doğrulama yöntemini DTO'ya ekliyoruz, böylece API hangi yöntemin seçildiğini bilebilir ve ona göre doğrulama yapabilir.
                Code = code
            });

            if (resetToken == null) // Eğer API doğrulama kodunu geçersiz veya süresi dolmuş olarak döndürürse, kullanıcıya hata mesajı gösteriyoruz.
            {
                ModelState.AddModelError("", "Geçersiz veya süresi dolmuş kod.");
                return View();
            }
            HttpContext.Session.SetString("reset_token", resetToken);// API'den dönen reset token'ını session'a kaydediyoruz, böylece sonraki adımlarda kullanabiliriz.
            HttpContext.Session.SetString("reset_verified", "true");// Doğrulama başarılı olduğunu belirten bir flag'i session'a kaydediyoruz, böylece kullanıcı yeni şifre belirleme sayfasına erişebilir.
            return RedirectToAction(nameof(SetNewPassword));
        }


        [HttpGet("/account/set-new-password")]
        public IActionResult SetNewPassword()
        {
            if (HttpContext.Session.GetString("reset_verified") != "true")// eğer yukarıda doğrulama başarılı olduğunu belirten flag session'da yoksa veya değeri "true" değilse, kullanıcıyı doğrudan login sayfasına yönlendiriyoruz. Bu, güvenlik için önemlidir çünkü doğrulama yapılmadan yeni şifre belirleme sayfasına erişilmesini engeller.
            {
                return RedirectToAction(nameof(AuthController.Login), AuthController.Name);
            }
            return View();
        }

        [HttpPost("/account/set-new-password")]
        public async Task<IActionResult> SetNewPassword(string confirmPassword, SetNewPasswordDto setNewPasswordDto)
        {
            if (HttpContext.Session.GetString("reset_verified") != "true")
            {
                return RedirectToAction(nameof(AuthController.Login), AuthController.Name);
            }
            if (setNewPasswordDto.NewPassword != confirmPassword)
            {
                ModelState.AddModelError("", "Şifreler eşleşmiyor.");
                return View();
            }

            setNewPasswordDto.Email = HttpContext.Session.GetString("reset_email")!; // Session'dan email bilgisini alıyoruz, böylece API'ye hangi kullanıcının şifresinin değiştirileceğini söyleyebiliriz.
            setNewPasswordDto.ResetToken = HttpContext.Session.GetString("reset_token")!; // Session'dan reset token'ını alıyoruz, böylece API'ye doğrulama kodunun geçerli olduğunu kanıtlayabiliriz.
            var ok = await _accountApiService.SetNewPasswordAsync(setNewPasswordDto);  // API'ye yeni şifre belirleme isteği gönderiyoruz ve işlemin başarılı olup olmadığını kontrol ediyoruz.
            if (!ok) 
            {
                ModelState.AddModelError("", "Şifreler değiştirilemedi.");
                return View();
            }
            HttpContext.Session.Clear();// Şifre değişikliği işlemi tamamlandıktan sonra, güvenlik için session'ı temizliyoruz. Böylece reset süreciyle ilgili tüm bilgiler (email, seçilen doğrulama yöntemi, reset token'ı vb.) session'dan kaldırılmış olur.
            TempData["Success"] = "Şifreniz başarıyla değiştirildi."; 
            return RedirectToAction(nameof(AuthController.Login), AuthController.Name);
        }

    }
}
