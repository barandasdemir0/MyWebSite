using DtoLayer.AuthDtos;
using DtoLayer.AuthDtos.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Enums;
using System.Security.Claims;
using WebUILayer.Areas.Admin.Services.Abstract;
using WebUILayer.Extension;
using WebUILayer.Services.Abstract;

namespace WebUILayer.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
[Route("[area]/[controller]/[action]/{id?}")]
public class SecurityController : Controller
{
    private readonly IUserProfileApiService _userProfileApiService;
    private readonly ITwoFactorApiService _twoFactorApiService;

    public SecurityController( IUserProfileApiService userProfileApiService, ITwoFactorApiService twoFactorApiService)
    {
        _userProfileApiService = userProfileApiService;
        _twoFactorApiService = twoFactorApiService;
    }


    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var profile = await _userProfileApiService.GetUserProfileAsync(); // Kullanıcı profil bilgilerini API'den alır
        if (profile==null)
        {
            return RedirectToAction("Register", "Auth");
        }
        return View(profile);
    }

    // Şifre değiştirme işlemi için POST metodu
    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
    {
        var ok = await _userProfileApiService.ChangePasswordAsync( changePasswordDto); // API üzerinden şifre değiştirme işlemini gerçekleştirir
        TempData[ok ? "Success" : "Error"] = ok // Sonuç durumuna göre kullanıcıya mesaj gösterir
           ? "Şifre başarıyla değiştirildi."
           : "Şifre değiştirilemedi. Mevcut şifrenizi kontrol edin.";
        return RedirectToAction("Index");
    }


    // 2FA açma/kapama işlemi için POST metodu
    [HttpPost]
    public async Task<IActionResult> Toggle2FA(Toggle2FADto toggle2FADto)
    {
        if (toggle2FADto.Enable && toggle2FADto.Provider == TwoFactorProvider.Authenticator) // Eğer 2FA açılmak isteniyor ve sağlayıcı Authenticator ise, kullanıcıyı kurulum sayfasına yönlendir
        {
            return RedirectToAction(nameof(SetupAuthenticator)); 
        }

        var ok = await _userProfileApiService.Toggle2FAAsync( toggle2FADto); // API üzerinden 2FA açma/kapama işlemini gerçekleştirir
        TempData[ok ? "Success" : "Error"] = ok
          ? (toggle2FADto.Enable ? "2FA başarıyla açıldı." : "2FA kapatıldı.")
          : "2FA ayarı değiştirilemedi.";
        return RedirectToAction(nameof(Index));

    }

    [HttpGet]
    public async Task<IActionResult> SetupAuthenticator()
    {
        var result = await _twoFactorApiService.SetupAuthenticatorAsync(); // API üzerinden Authenticator kurulum bilgilerini alır (QR kodu, gizli anahtar vb.)
        return View(result);
    }


    // Authenticator doğrulama işlemi için POST metodu
    [HttpPost]
    public async Task<IActionResult> ConfirmAuthenticator(string code)
    {
        // API üzerinden Authenticator doğrulama işlemini gerçekleştirir. Kullanıcının girdiği kodu ve kullanıcı ID'sini gönderir.
        var ok = await _twoFactorApiService.ConfirmAuthenticatorAsync(new TwoFactorVerifyDto
        {
            UserId = User.GetUserId(),
            Code = code,
            Provider = TwoFactorProvider.Authenticator
        });
        if (ok)
        {
            TempData["Success"] = "Authenticator başarıyla kuruldu ve 2FA açıldı.";
            return RedirectToAction("Index");
        }
        TempData["Error"] = "Geçersiz kod. Tekrar deneyin.";
        return RedirectToAction(nameof(SetupAuthenticator));
    }

}
