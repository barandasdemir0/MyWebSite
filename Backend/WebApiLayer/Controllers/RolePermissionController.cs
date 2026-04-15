using BusinessLayer.Abstract;
using CV.EntityLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApiLayer.Controllers;

[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
[ApiController]
public class RolePermissionController : ControllerBase
{

    private readonly IRolePermissionService _rolePermissionService;

    public RolePermissionController(IRolePermissionService rolePermissionService)
    {
        _rolePermissionService = rolePermissionService;
    }

    [HttpGet("{roleName}")]
    [Authorize(Roles = RoleConsts.Admin)]
    public async Task<IActionResult> GetRolePermissions(string roleName, CancellationToken cancellationToken)
    {
        var perms = await _rolePermissionService.GetRolePermissionsAsync(roleName, cancellationToken);
        return Ok(perms);
    }

    [HttpPost("{roleName}")]
    [Authorize(Roles = RoleConsts.Admin)]
    public async Task<IActionResult> SaveRolePermissions(string roleName, [FromBody] List<string> permissions, CancellationToken cancellation)
    {
        var ok = await _rolePermissionService.SaveRolePermissionsAsync(roleName, permissions, cancellation);
        if (ok)
        {
            return Ok();
        }
        return BadRequest();
    }
}
