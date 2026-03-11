using BusinessLayer.Abstract;
using DtoLayer.AuthDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApiLayer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserProfileController : ControllerBase
{
    private readonly IUserProfileService _userProfileService;

    public UserProfileController(IUserProfileService userProfileService)
    {
        _userProfileService = userProfileService;
    }

    [HttpPost("assign-role")]
    [Authorize]
    public async Task<IActionResult> AssignRole([FromBody] AssignRoleDto assignRoleDto, CancellationToken cancellationToken)
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

    [HttpGet("profile")]
    [Authorize]
    public async Task<IActionResult> GetProfile(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var profile = await _userProfileService.GetUserProfileAsync(userId, cancellationToken);
        return Ok(profile);
    }

    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var ok = await _userProfileService.ChangePasswordAsync(userId, changePasswordDto, cancellationToken);
        if (ok)
        {
            return Ok();
        }
        return BadRequest("Şifre değiştirilemedi mevcut şifren yanlış olabilir");
    }

    [HttpPost("toggle-2fa")]
    [Authorize]
    public async Task<IActionResult> Toggle2FA([FromBody] Toggle2FADto toggle2FADto, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var ok = await _userProfileService.Toggle2FAAsync(userId, toggle2FADto, cancellationToken);
        if (ok)
        {
            return Ok();
        }
        return BadRequest("2FA ayarı değiştirilemedi");
    }

}
