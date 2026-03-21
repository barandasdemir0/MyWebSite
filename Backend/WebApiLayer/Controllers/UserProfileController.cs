using BusinessLayer.Abstract;
using DtoLayer.AuthDtos;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApiLayer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserProfileController : ControllerBase
{
    private readonly IUserProfileService _userProfileService;
    private readonly IUserAdminService _userAdminService;
    private readonly IRolePermissionService _rolePermissionService;
    private readonly ITwoFactorService _twoFactorService;

    public UserProfileController(IUserProfileService userProfileService, IUserAdminService userAdminService, IRolePermissionService rolePermissionService, ITwoFactorService twoFactorService)
    {
        _userProfileService = userProfileService;
        _userAdminService = userAdminService;
        _rolePermissionService = rolePermissionService;
        _twoFactorService = twoFactorService;
    }

    [HttpPost("assign-role")]
    [Authorize(Roles = RoleConsts.Admin)]
    public async Task<IActionResult> AssignRole([FromBody] AssignRoleDto assignRoleDto, CancellationToken cancellationToken)
    {
        var ok = await _userAdminService.AssignRoleAsync(assignRoleDto.UserId, assignRoleDto.Role, cancellationToken);
        if (ok)
        {
            return Ok();
        }
        else
        {
            return BadRequest("Rol atanamadı");
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
        var ok = await _twoFactorService.Toggle2FAAsync(userId, toggle2FADto, cancellationToken);
        if (ok)
        {
            return Ok();
        }
        return BadRequest("2FA ayarı değiştirilemedi");
    }

    [HttpGet("pending-users")]
    [Authorize(Roles = RoleConsts.Admin)]
    public async Task<IActionResult> GetPendingUsers(CancellationToken cancellationToken)
    {
        var users = await _userAdminService.GetPendingUsersAsync(cancellationToken);
        return Ok(users);
    }

    [HttpPost("approve-user/{userId}")]
    [Authorize(Roles = RoleConsts.Admin)]
    public async Task<IActionResult> ApproveUser(string userId,[FromBody] string role , CancellationToken cancellationToken)
    {
        var ok = await _userAdminService.ApproveUserAsync(userId, role, cancellationToken);
        if (ok)
        {
            return Ok();
        }
        return BadRequest("Rol onaylanamadı");
    }

    [HttpPost("reject-user/{userId}")]
    [Authorize(Roles = RoleConsts.Admin)]
    public async Task<IActionResult> RejectUser(string userId, CancellationToken cancellationToken)
    {
        var ok = await _userAdminService.RejectUserAsync(userId, cancellationToken);
        if (ok)
        {
            return Ok();
        }
        return BadRequest("Kullanıcı Silinemedi");
    }

    [HttpGet("all-users")]
    [Authorize(Roles = RoleConsts.Admin)]
    public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken)
    {
        var users = await _userAdminService.GetAllUserAsync(cancellationToken);
        return Ok(users);
    }

    [HttpGet("role-permissions/{roleName}")] 
    [Authorize(Roles = RoleConsts.Admin)]
    public async Task<IActionResult> GetRolePermissions(string roleName,CancellationToken cancellationToken)
    {
        var perms = await _rolePermissionService.GetRolePermissionsAsync(roleName, cancellationToken);
        return Ok(perms);
    }

    [HttpPost("role-permissions/{roleName}")] 
    [Authorize(Roles = RoleConsts.Admin)]
    public async Task<IActionResult> SaveRolePermissions(string roleName,[FromBody]List<string> permissions,CancellationToken cancellation)
    {
        var ok = await _rolePermissionService.SaveRolePermissionsAsync(roleName, permissions, cancellation);
        if (ok)
        {
            return Ok();
        }
        return BadRequest();
    }

}
