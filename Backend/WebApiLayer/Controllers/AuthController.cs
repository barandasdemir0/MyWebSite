using BusinessLayer.Abstract;
using DtoLayer.AuthDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApiLayer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{

    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto,CancellationToken cancellationToken)
    {
        var result = await _authService.LoginAsync(loginDto, cancellationToken);
        if (result.Success)
        {
            return Ok(result);
        }
        else
        {
            return Unauthorized(result.Error);
        }
    }


    [HttpPost("send-email-code")]
    public async Task<IActionResult> SendEmailCode([FromBody] string userId,CancellationToken cancellationToken)
    {
        var sent = await _authService.SendMailOtpAsync(userId, cancellationToken);
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
    public async Task<IActionResult> VerifyTwoFactor([FromBody] TwoFactorVerifyDto dto,CancellationToken cancellationToken)
    {
        var result = await _authService.VerifyTwoFactorAsync(dto, cancellationToken);
        if (result.Success)
        {
            return Ok(result);
        }
        else
        {
            return BadRequest(result.Error);
        }
    }

    [HttpGet("setup-authenticator/{userId}")]
    [Authorize]
    public async Task<IActionResult> SetupAuthenticator(string userId,CancellationToken cancellationToken)
    {
        var result = await _authService.SetupAuthenticatorAsync(userId, cancellationToken);
        return Ok(result);
    }


    [HttpPost("Confirm-authenticator")]
    [Authorize]
    public async Task<IActionResult> ConfirmAuthenticator([FromBody] TwoFactorVerifyDto twoFactorVerifyDto,CancellationToken cancellation)
    {
        var ok = await _authService.ConfirmAuthenticatorSetupAsync(twoFactorVerifyDto.UserId, twoFactorVerifyDto.Code, cancellation);
        if (ok)
        {
            return Ok();
        }
        else
        {
            return BadRequest("Geçersiz kod, authenticator kurulumu başarısız");
        }
    }


    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> LogOut(CancellationToken cancellation)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        await _authService.RevokeTokensAsync(userId, cancellation);
        return Ok();

    }






}
