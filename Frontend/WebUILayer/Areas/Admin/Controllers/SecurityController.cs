using DtoLayer.AuthDtos;
using EntityLayer.Entities;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebUILayer.Areas.Admin.Services.Abstract;

namespace WebUILayer.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
[Route("[area]/[controller]/[action]/{id?}")]
public class SecurityController : Controller
{
    private readonly IAuthApiService _authApiService;
    private readonly IUserProfileApiService _userProfileApiService;
    private readonly ITwoFactorApiService _twoFactorApiService;

    public SecurityController(IAuthApiService authApiService, IUserProfileApiService userProfileApiService, ITwoFactorApiService twoFactorApiService)
    {
        _authApiService = authApiService;
        _userProfileApiService = userProfileApiService;
        _twoFactorApiService = twoFactorApiService;
    }

    private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;


    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var profile = await _userProfileApiService.GetUserProfileAsync(GetUserId());
        return View(profile);
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
    {
        var ok = await _userProfileApiService.ChangePasswordAsync(GetUserId(), changePasswordDto);
        TempData[ok ? "Success" : "Error"] = ok
           ? "Şifre başarıyla değiştirildi."
           : "Şifre değiştirilemedi. Mevcut şifrenizi kontrol edin.";
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Toggle2FA(Toggle2FADto toggle2FADto)
    {
        if (toggle2FADto.Enable && toggle2FADto.Provider == TwoFactorProvider.Authenticator)
        {
            return RedirectToAction(nameof(SetupAuthenticator));
        }

        var ok = await _userProfileApiService.Toggle2FAAsync(GetUserId(), toggle2FADto);
        TempData[ok ? "Success" : "Error"] = ok
          ? (toggle2FADto.Enable ? "2FA başarıyla açıldı." : "2FA kapatıldı.")
          : "2FA ayarı değiştirilemedi.";
        return RedirectToAction(nameof(Index));

    }

    [HttpGet]
    public async Task<IActionResult> SetupAuthenticator(string code)
    {
        var result = await _twoFactorApiService.SetupAuthenticatorAsync(GetUserId());
        return View(result);
    }


    [HttpPost]
    public async Task<IActionResult> ConfirmAuthenticator(string code)
    {
        var ok = await _twoFactorApiService.ConfirmAuthenticatorAsync(new TwoFactorVerifyDto
        {
            UserId = GetUserId(),
            Code = code
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
