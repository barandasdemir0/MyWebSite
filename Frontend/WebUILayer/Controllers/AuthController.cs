using DtoLayer.AuthDtos;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebUILayer.Areas.Admin.Services.Abstract;

namespace WebUILayer.Controllers;

public class AuthController : Controller
{
    private readonly IAuthApiService _authApiService;
    private readonly ITwoFactorApiService _twoFactorApiService;

    public AuthController(IAuthApiService authApiService, ITwoFactorApiService twoFactorApiService)
    {
        _authApiService = authApiService;
        _twoFactorApiService = twoFactorApiService;
    }

    [HttpGet("/auth/login")]
    public IActionResult Login() => View();

    [HttpPost("/auth/login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {

        var result = await _authApiService.LoginAsync(loginDto);
        if (result == null || !result.Success)
        {
            ModelState.AddModelError("", result?.Error ?? "Bağlantı Hatası");
            return View(loginDto);
        }
        if (result.RequiresTwoFactor)
        {
            HttpContext.Session.SetString("2FA_UserId", result.UserId!);
            return RedirectToAction("ChooseTwoFactor");
        }
        await SetCookieFromJwtAsync(result.Token!);
        return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
        //return LocalRedirect("/Admin/Dashboard/Index");

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
    public async Task<IActionResult> ProcessTwoFactorChoice(string SelectedProvider)
    {
        var userId = HttpContext.Session.GetString("2FA_UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction(nameof(Login));
        }
        if (SelectedProvider == "Email")
        {
            return RedirectToAction(nameof(SendEmailCode));
        }
        HttpContext.Session.SetString("2FA_Provider", "Authenticator");
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
    public IActionResult VerifyTwoFactor() => View();

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
                Provider = Enum.Parse<TwoFactorProvider>(provider)
            }
            );
        if (result == null || ! result.Success)
        {
            ModelState.AddModelError("", result?.Error ?? "Geçersiz Kod");
            return View();
        }
        await SetCookieFromJwtAsync(result.Token!);
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
    }





    [HttpPost("/auth/logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        HttpContext.Response.Cookies.Delete("AccessToken");
        return RedirectToAction(nameof(Login));
    }


    private async Task SetCookieFromJwtAsync(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        var identity = new ClaimsIdentity
            (
                jwt.Claims,
                CookieAuthenticationDefaults.AuthenticationScheme
            );

        await HttpContext.SignInAsync
            (
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity),
                new AuthenticationProperties
                {
                    IsPersistent = true
                }
            );

        HttpContext.Response.Cookies.Append("AccessToken", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddHours(1)
        });
     
    }


}
