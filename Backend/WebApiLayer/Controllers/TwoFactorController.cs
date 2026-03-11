using BusinessLayer.Abstract;
using DtoLayer.AuthDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace WebApiLayer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TwoFactorController : ControllerBase
{
    private readonly ITwoFactorService _twoFactorService;

    public TwoFactorController(ITwoFactorService twoFactorService)
    {
        _twoFactorService = twoFactorService;
    }

    [EnableRateLimiting("email")]
    [HttpPost("send-email-code")]
    public async Task<IActionResult> SendEmailCode([FromBody] string userId, CancellationToken cancellationToken)
    {
        var sent = await _twoFactorService.SendMailOtpAsync(userId, cancellationToken);
        if (sent)
        {
            return Ok();
        }
        else
        {
            return BadRequest("Kod Gönderilemedi");
        }
    }


    [HttpPost("verify-2fa")]
    public async Task<IActionResult> VerifyTwoFactor([FromBody] TwoFactorVerifyDto dto, CancellationToken cancellationToken)
    {
        var result = await _twoFactorService.VerifyTwoFactorAsync(dto, cancellationToken);
        if (result.Success)
        {
            return Ok(result);
        }
        else
        {
            return BadRequest(result.Error);
        }
    }

    [HttpGet("setup-authenticator")]
    [Authorize]
    public async Task<IActionResult> SetupAuthenticator(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var result = await _twoFactorService.SetupAuthenticatorAsync(userId, cancellationToken);
        return Ok(result);
    }


    [HttpPost("Confirm-authenticator")]
    [Authorize]
    public async Task<IActionResult> ConfirmAuthenticator([FromBody] TwoFactorVerifyDto twoFactorVerifyDto, CancellationToken cancellation)
    {
        var ok = await _twoFactorService.ConfirmAuthenticatorSetupAsync(twoFactorVerifyDto.UserId, twoFactorVerifyDto.Code, cancellation);
        if (ok)
        {
            return Ok();
        }
        else
        {
            return BadRequest("Geçersiz kod, authenticator kurulumu başarısız");
        }
    }
}
