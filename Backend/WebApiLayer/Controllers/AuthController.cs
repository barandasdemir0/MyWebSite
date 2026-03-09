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
    private readonly IUserProfileService _userProfileService;
    private readonly ITwoFactorService _twoFactorService;

    public AuthController(IAuthService authService, IUserProfileService userProfileService, ITwoFactorService twoFactorService)
    {
        _authService = authService;
        _userProfileService = userProfileService;
        _twoFactorService = twoFactorService;
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
    public async Task<IActionResult> VerifyTwoFactor([FromBody] TwoFactorVerifyDto dto,CancellationToken cancellationToken)
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

    [HttpGet("setup-authenticator/{userId}")]
    [Authorize]
    public async Task<IActionResult> SetupAuthenticator(string userId,CancellationToken cancellationToken)
    {
        var result = await _twoFactorService.SetupAuthenticatorAsync(userId, cancellationToken);
        return Ok(result);
    }


    [HttpPost("Confirm-authenticator")]
    [Authorize]
    public async Task<IActionResult> ConfirmAuthenticator([FromBody] TwoFactorVerifyDto twoFactorVerifyDto,CancellationToken cancellation)
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

    [HttpPost("assign-role")]
    [Authorize]
    public async Task<IActionResult> AssignRole([FromBody] AssignRoleDto assignRoleDto,CancellationToken cancellationToken)
    {
        var ok = await _userProfileService.AssignRoleAsync(assignRoleDto.UserId, assignRoleDto.Role, cancellationToken);
        if (ok)
        {
            return Ok();
        }
        else
        {
            return BadRequest();
        }
    }

    [HttpGet("profile/{userId}")]
    [Authorize]
    public async Task<IActionResult> GetProfile(string userId , CancellationToken cancellationToken)
    {
        var profile = await _userProfileService.GetUserProfileAsync(userId, cancellationToken);
        return Ok(profile);
    }

    [HttpPost("change-password/{userId}")]
    [Authorize]
    public async Task<IActionResult> ChangePassword(string userId, [FromBody] ChangePasswordDto changePasswordDto,CancellationToken cancellationToken)
    {
        var ok = await _userProfileService.ChangePasswordAsync(userId, changePasswordDto, cancellationToken);
        if (ok)
        {
            return Ok();
        }
        return BadRequest("Şifre değiştirilemedi mevcut şifren yanlış olabilir");
    }

    [HttpPost("toggle-2fa/{userId}")]
    [Authorize]
    public async Task<IActionResult> Toggle2FA(string userId, [FromBody] Toggle2FADto toggle2FADto ,CancellationToken cancellationToken)
    {
        var ok = await _userProfileService.Toggle2FAAsync(userId, toggle2FADto, cancellationToken);
        if (ok)
        {
            return Ok();
        }
        return BadRequest("2FA ayarı değiştirilemedi");
    }





}
