using BusinessLayer.Abstract;
using DtoLayer.AuthDtos.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace WebApiLayer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    //şifre sıfırlama süreci 3 adımda gerçekleşir

    //1. adım: kullanıcı eposta adresini gönderir, eğer bu eposta kayıtlıysa sıfırlama bağlantısı gönderilir
    [HttpPost("forgot-password")]
    [EnableRateLimiting("email")]//bu endpoint'e çok fazla istek gelmesini önlemek için rate limiting uygulanır
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto, CancellationToken cancellationToken) //frombody kullanıcının gönderdiği veriyi alır ve forgotPasswordDto nesnesine dönüştürür
    {
        await _accountService.ForgotPasswordAsync(forgotPasswordDto.Email, cancellationToken); //bu method eposta adresini alır ve eğer bu eposta kayıtlıysa sıfırlama bağlantısı gönderir
        return Ok("Eğer bu eposta kayıtlıysa,sıfırlama bağlantısı gönderildi");
    }

 
    //2. adım: kullanıcı eposta adresine gelen kodu ve sağlayıcıyı gönderir, eğer kod geçerliyse sıfırlama token'ı döner
    [HttpPost("verify-reset-otp")]
    [EnableRateLimiting("email")]//bu endpoint'e çok fazla istek gelmesini önlemek için rate limiting uygulanır
    public async Task<IActionResult> VerifyResetOtp([FromBody] VerifyResetOtpDto verifyResetOtpDto, CancellationToken cancellationToken)
    {
        var ok = await _accountService.VerifyResetOtpAsync(verifyResetOtpDto.Email, verifyResetOtpDto.Code, verifyResetOtpDto.Provider, cancellationToken); //bu method eposta adresi, kod ve sağlayıcıyı alır ve eğer kod geçerliyse sıfırlama token'ı döner
        if (ok != null)
        {
            return Ok(new
            {
                resetToken = ok //bu token daha sonra şifre sıfırlama işlemi için kullanılacak
            });
        }
        return BadRequest("Geçersiz veya süresi dolmuş kod");
    }

    //3. adım: kullanıcı yeni şifresini, eposta adresini ve sıfırlama token'ını gönderir, eğer token geçerliyse şifre sıfırlanır
    [HttpPost("set-new-password")]
    [EnableRateLimiting("email")]
    public async Task<IActionResult> SetNewPassword([FromBody] SetNewPasswordDto setNewPasswordDto, CancellationToken cancellationToken)
    {
        var ok = await _accountService.SetNewPasswordAsync(setNewPasswordDto.Email, setNewPasswordDto.NewPassword, setNewPasswordDto.ResetToken, cancellationToken); //bu method eposta adresi, yeni şifre ve sıfırlama token'ını alır ve eğer token geçerliyse şifre sıfırlanır
        if (ok)
        {
            return Ok();
        }
        return BadRequest("şifre değiştirilemedi veya token geçersiz");
    }
}
