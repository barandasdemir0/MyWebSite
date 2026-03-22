using BusinessLayer.Abstract;
using DtoLayer.AuthDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
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

  


    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> LogOut(CancellationToken cancellation)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        await _authService.RevokeTokensAsync(userId, cancellation);
        return Ok();

    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto,CancellationToken cancellationToken)
    {
        var result = await _authService.RegisterAsync(registerDto, cancellationToken);
        if (result.Success)
        {
            return Ok(result);
        }
        else
        {
            return BadRequest(result.Errors);
        }
    }



    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto dto,CancellationToken cancellationToken)
    {
        var deviceInfo = Request.Headers["User-Agent"].ToString();
        var result = await _authService.RefreshTokenAsync(dto, deviceInfo, cancellationToken);
        if (result.Success)
        {
            return Ok(result);
        }
        return Unauthorized(result.Error);
    }


    [HttpPost("forgot-password")]
    [EnableRateLimiting("email")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto,CancellationToken cancellationToken)
    {
        await _authService.ForgotPasswordAsync(forgotPasswordDto.Email, cancellationToken);
        return Ok("Eğer bu eposta kayıtlıysa,sıfırlama bağlantısı gönderildi");
    }


    [HttpPost("verify-reset-otp")]
    [EnableRateLimiting("email")]
    public async Task<IActionResult> VerifyResetOtp([FromBody]VerifyResetOtpDto verifyResetOtpDto,CancellationToken cancellationToken)
    {
        var ok = await _authService.VerifyResetOtpAsync(verifyResetOtpDto.Email,verifyResetOtpDto.Code,verifyResetOtpDto.Provider,cancellationToken);
        if (ok!=null)
        {
            return Ok(new
            {
                resetToken = ok
            });
        }
        return BadRequest("Geçersiz veya süresi dolmuş kod");
    }

    [HttpPost("set-new-password")]
    [EnableRateLimiting("email")]
    public async Task<IActionResult> SetNewPassword([FromBody] SetNewPasswordDto setNewPasswordDto,CancellationToken cancellationToken)
    {
        var ok = await _authService.SetNewPasswordAsync(setNewPasswordDto.Email, setNewPasswordDto.NewPassword, setNewPasswordDto.ResetToken ,cancellationToken);
        if (ok)
        {
            return Ok();
        }
        return BadRequest("şifre değiştirilemedi veya token geçersiz");
    }


}
