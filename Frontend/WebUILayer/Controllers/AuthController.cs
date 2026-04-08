using DtoLayer.AuthDtos;
using DtoLayer.AuthDtos.Requests;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Enums;
using WebUILayer.Services.Abstract;

namespace WebUILayer.Controllers;

public class AuthController : Controller
{
    private readonly IAuthApiService _authApiService;
    private readonly ITwoFactorApiService _twoFactorApiService;
    private readonly ICookieAuthService _cookieAuthService;

    public AuthController(IAuthApiService authApiService, ITwoFactorApiService twoFactorApiService, ICookieAuthService cookieAuthService)
    {
        _authApiService = authApiService;
        _twoFactorApiService = twoFactorApiService;
        _cookieAuthService = cookieAuthService;
    }

    [HttpGet("/auth/login")]
    public IActionResult Login() => View();

    [HttpPost("/auth/login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        loginDto.DeviceInfo = Request.Headers["User-Agent"].ToString();
        var result = await _authApiService.LoginAsync(loginDto);
        if (result == null || !result.Success)
        {
            ModelState.AddModelError("", result?.Error ?? "Bağlantı Hatası");
            return View(loginDto);
        }
        if (result.RequiresTwoFactor)
        {
            HttpContext.Session.SetString("2FA_UserId", result.UserId!);
            return RedirectToAction(nameof(ChooseTwoFactor));
        }
        await _cookieAuthService.SignInWithJwtAsync(result.Token!,result.RefreshToken,loginDto.RememberMe);
        return RedirectToAction("Index", "Dashboard", new { area = "Admin" });

    }




    [HttpGet("/auth/register")]
    public IActionResult Register() => View();

    [HttpPost("/auth/register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        var result = await _authApiService.RegisterAsync(registerDto);
        if (result == null || !result.Success)
        {
            foreach (var error in result?.Errors ?? [])
            {
                ModelState.AddModelError("", error);
            }
            return View(registerDto);
        }
        TempData["Success"] = "Kayıt başarılı, giriş yapabilirsiniz.";
        return RedirectToAction(nameof(Login));
    }

    




    [HttpGet("/auth/choose-2fa")]
    public IActionResult ChooseTwoFactor() => View();


    [HttpPost("/auth/process-2fa-choice")]
    public async Task<IActionResult> ProcessTwoFactorChoice(TwoFactorProvider SelectedProvider)
    {
        var userId = HttpContext.Session.GetString("2FA_UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction(nameof(Login));
        }
        if (SelectedProvider==TwoFactorProvider.Email)
        {
            return RedirectToAction(nameof(SendEmailCode));
        }
        HttpContext.Session.SetString("2FA_Provider", SelectedProvider.ToString());
        return RedirectToAction(nameof(VerifyTwoFactor));
    }




    [HttpGet("/auth/send-email-code")]
    public async Task<IActionResult> SendEmailCode()
    {
        var userId = HttpContext.Session.GetString("2FA_UserId");
        if (string.IsNullOrEmpty(userId)) return RedirectToAction(nameof(Login));
        await _twoFactorApiService.SendEmailCodeAsync(userId!);
        HttpContext.Session.SetString("2FA_Provider", "Email");
        return RedirectToAction(nameof(VerifyTwoFactor));

    }

    [HttpGet("/auth/verify-2fa")]
    public IActionResult VerifyTwoFactor() 
    {
        var provider = HttpContext.Session.GetString("2FA_Provider") ?? "Email";
        ViewBag.provider = provider;
        return View();
    }

    [HttpPost("/auth/verify-2fa")]
    public async Task<IActionResult> VerifyTwoFactor(string code)
    {
        var userId = HttpContext.Session.GetString("2FA_UserId");
        var provider = HttpContext.Session.GetString("2FA_Provider") ?? "Email";

        var result = await _twoFactorApiService.VerifyTwoFactorAsync
            (
            new TwoFactorVerifyDto
            {
                UserId = userId!,
                Code = code,
                Provider = Enum.Parse<TwoFactorProvider>(provider),
                DeviceInfo = Request.Headers["User-Agent"].ToString()
            }
            );
        if (result == null || ! result.Success)
        {
            ModelState.AddModelError("", result?.Error ?? "Geçersiz Kod");
            return View();
        }
        await _cookieAuthService.SignInWithJwtAsync(result.Token!,result.RefreshToken);
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
    }





    [HttpPost("/auth/logout")]
    public async Task<IActionResult> Logout()
    {
        await _cookieAuthService.SignOutAsync();
        return RedirectToAction(nameof(Login));
    }


    [HttpGet("/auth/forgot-password")]
    public IActionResult ForgotPassword() => View();

    [HttpPost("/auth/forgot-password")]
    public async Task< IActionResult> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
    {
        // Not: Burada API çağrısı yapılmıyor çünkü kullanıcı önce doğrulama yöntemini
        // seçecek (Email/Authenticator). OTP gönderimi ProcessResetChoice'da yapılıyor.

        HttpContext.Session.SetString("reset_email", forgotPasswordDto.Email);
        return RedirectToAction(nameof(ChooseResetMethod));
    }

    [HttpGet("/auth/ChooseResetMethod")]
    public IActionResult ChooseResetMethod()
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("reset_email")))
        {
            return RedirectToAction(nameof(Login));
        }
        return View();
    }

    [HttpPost("/auth/process-reset-choice")]
    public async Task<IActionResult> ProcessResetChoice(string SelectedProvider)
    {
        var email = HttpContext.Session.GetString("reset_email");
        if (string.IsNullOrEmpty(email))
        {
            return RedirectToAction(nameof(Login));
        }
        if (SelectedProvider=="Email")
        {
            await _authApiService.ForgotPasswordAsync(new ForgotPasswordDto
            {
                Email = email
            });
        }

        HttpContext.Session.SetString("reset_provider", SelectedProvider);
        return RedirectToAction(nameof(VerifyResetCode));
    }


    [HttpGet("/auth/resend-reset-code")]
    public async Task<IActionResult> ResendResetCode()
    {
        var email = HttpContext.Session.GetString("reset_email");
        if (string.IsNullOrEmpty(email)) return RedirectToAction(nameof(Login));

        await _authApiService.ForgotPasswordAsync(new ForgotPasswordDto { Email = email });
        TempData["Info"] = "Kod tekrar gönderildi.";
        return RedirectToAction(nameof(VerifyResetCode));
    }




    [HttpGet("/auth/verify-reset-code")]
    public IActionResult VerifyResetCode()
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("reset_email")))
        {
            return RedirectToAction(nameof(Login));
        }
        return View();
    }

    [HttpPost("/auth/verify-reset-code")]
    public async Task<IActionResult> VerifyResetCode(string code)
    {
        var email = HttpContext.Session.GetString("reset_email");
        var providerStr = HttpContext.Session.GetString("reset_provider") ?? "Email";
        if (string.IsNullOrEmpty(email))
        {
            return RedirectToAction(nameof(Login));
        }
        var provider = Enum.Parse<TwoFactorProvider>(providerStr);
        var resetToken = await _authApiService.VerifyResetOtpAsync(new VerifyResetOtpDto
        {
            Email = email,
            Provider = provider,
            Code = code
        });

        if (resetToken==null)
        {
            ModelState.AddModelError("", "Geçersiz veya süresi dolmuş kod.");
            return View();
        }
        HttpContext.Session.SetString("reset_token", resetToken);
        HttpContext.Session.SetString("reset_verified", "true");
        return RedirectToAction(nameof(SetNewPassword));
    }


    [HttpGet("/auth/set-new-password")]
    public IActionResult SetNewPassword()
    {
        if (HttpContext.Session.GetString("reset_verified")!="true")
        {
            return RedirectToAction(nameof(Login));
        }
        return View();
    }

    [HttpPost("/auth/set-new-password")]
    public async Task<IActionResult> SetNewPassword(string confirmPassword,SetNewPasswordDto setNewPasswordDto)
    {
        if (HttpContext.Session.GetString("reset_verified") != "true")
        {
            return RedirectToAction(nameof(Login));
        }
        if (setNewPasswordDto.NewPassword != confirmPassword)
        {
            ModelState.AddModelError("", "Şifreler eşleşmiyor.");
            return View();
        }

        setNewPasswordDto.Email = HttpContext.Session.GetString("reset_email")!;
        setNewPasswordDto.ResetToken = HttpContext.Session.GetString("reset_token")!;
        var ok = await _authApiService.SetNewPasswordAsync(setNewPasswordDto);
        if (!ok)
        {
            ModelState.AddModelError("", "Şifreler değiştirilemedi.");
            return View();
        }
        HttpContext.Session.Clear();
        TempData["Success"] = "Şifreniz başarıyla değiştirildi.";
        return RedirectToAction(nameof(Login));
    }






 


}
