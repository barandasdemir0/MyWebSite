using DtoLayer.AuthDtos;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WebUILayer.Areas.Admin.Services.Abstract;

namespace WebUILayer.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
[Route("[area]/[controller]/[action]/{id?}")]
public class UserManagementController : Controller
{
    private readonly IUserAdminApiService _userAdminApiService;
    private readonly IRolePermissionApiService _rolePermissionApiService;


    public UserManagementController( IUserAdminApiService userAdminApiService, IRolePermissionApiService rolePermissionApiService)
    {
        _userAdminApiService = userAdminApiService;
        _rolePermissionApiService = rolePermissionApiService;
    }

    [HttpGet]
    public async Task<IActionResult> PendingUser(string selectedRole = RoleConsts.Editor)
    {
        var pending = await _userAdminApiService.GetPendingUsersAsync();
        var approved = await _userAdminApiService.GetAllUsersAsync();
        var perms = await _rolePermissionApiService.GetRolePermissionsAsync(selectedRole);
        var vm = new UserManagementViewModel
        {
            PendingUsers = pending,
            ApprovedUsers = approved,
            SelectedRole = selectedRole,
            ActivePermissions = perms,
            AllPermissions = PermissionConsts.GetAll().Select(p => new PermissionItem { Key = p.Key, Label = p.Label }).ToList(),
            AllRoles = new List<string> { RoleConsts.User, RoleConsts.Editor, RoleConsts.Admin },
            SelectableRoles = new List<string> { RoleConsts.Editor, RoleConsts.User }
        };
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Approve(string id, string role = RoleConsts.User)
    {
        var ok = await _userAdminApiService.ApproveUserAsync(id, role);
        TempData[ok ? "Success" : "Error"] = ok
            ? "Kullanıcı onaylandı." : "Kullanıcı onaylanamadı.";
        return RedirectToAction(nameof(PendingUser));
    }

    [HttpPost]
    public async Task<IActionResult> Reject(string id)
    {
        var ok = await _userAdminApiService.RejectUserAsync(id);
        TempData[ok ? "Success" : "Error"] = ok ? "Kullanıcı reddedildi." : "Reddetme başarısız.";
        return RedirectToAction(nameof(PendingUser));

    }

    [HttpGet]
    public async Task<IActionResult> ManageUsers()
    {
        var users = await _userAdminApiService.GetAllUsersAsync();
        return View(users);
    }
    [HttpGet]
    public async Task<IActionResult> GetRolePermissions(string roleName)
    {
        var perms = await _rolePermissionApiService.GetRolePermissionsAsync(roleName);
        return Json(perms);
    }
    [HttpPost]
    public async Task<IActionResult> SaveRolePermissions(
        string roleName, [FromBody] List<string> permissions)
    {
        var ok = await _rolePermissionApiService.SaveRolePermissions(roleName, permissions);
        return ok ? Ok() : BadRequest();
    }

    [HttpPost]
    public async Task<IActionResult> SavePermissions(string selectedRole, List<string> permissions)
    {
        var ok = await _rolePermissionApiService.SaveRolePermissions(selectedRole, permissions ?? new());
        TempData[ok ? "Success" : "Error"] = ok
            ? "İzinler kaydedildi." : "İzinler kaydedilemedi.";
        return RedirectToAction(nameof(PendingUser), new { selectedRole });
    }

}
