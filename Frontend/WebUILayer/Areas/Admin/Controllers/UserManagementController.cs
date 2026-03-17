using DtoLayer.AuthDtos;
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
    private readonly IUserProfileApiService _userProfileApiService;

    public UserManagementController(IUserProfileApiService userProfileApiService)
    {
        _userProfileApiService = userProfileApiService;
    }

    [HttpGet]
    public async Task<IActionResult> PendingUser(string selectedRole = "Editor")
    {
        var pending = await _userProfileApiService.GetPendingUsersAsync();
        var approved = await _userProfileApiService.GetAllUsersAsync();
        var perms = await _userProfileApiService.GetRolePermissionsAsync(selectedRole);
        var vm = new UserManagementViewModel
        {
            PendingUsers = pending,
            ApprovedUsers = approved,
            SelectedRole = selectedRole,
            ActivePermissions = perms,
            AllPermissions = new List<PermissionItem>
        {
            new() { Key = "blog.create",     Label = "Blog oluştur" },
            new() { Key = "blog.edit",       Label = "Blog düzenle" },
            new() { Key = "blog.delete",     Label = "Blog sil" },
            new() { Key = "project.create",  Label = "Proje oluştur" },
            new() { Key = "project.edit",    Label = "Proje düzenle" },
            new() { Key = "project.delete",  Label = "Proje sil" },
            new() { Key = "skill.manage",    Label = "Yetenekler" },
            new() { Key = "user.manage",     Label = "Kullanıcı yönetimi" },
            new() { Key = "site.settings",   Label = "Site ayarları" },
        }
        };
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Approve(string id, string role = "User")
    {
        var ok = await _userProfileApiService.ApproveUserAsync(id, role);
        TempData[ok ? "Success" : "Error"] = ok
            ? "Kullanıcı onaylandı." : "Kullanıcı onaylanamadı.";
        return RedirectToAction(nameof(PendingUser));
    }

    [HttpPost]
    public async Task<IActionResult> Reject(string id)
    {
        var ok = await _userProfileApiService.RejectUserAsync(id);
        TempData[ok ? "Success" : "Error"] = ok ? "Kullanıcı reddedildi." : "Reddetme başarısız.";
        return RedirectToAction(nameof(PendingUser));

    }

    [HttpGet]
    public async Task<IActionResult> ManageUsers()
    {
        var users = await _userProfileApiService.GetAllUsersAsync();
        return View(users);
    }
    [HttpGet]
    public async Task<IActionResult> GetRolePermissions(string roleName)
    {
        var perms = await _userProfileApiService.GetRolePermissionsAsync(roleName);
        return Json(perms);
    }
    [HttpPost]
    public async Task<IActionResult> SaveRolePermissions(
        string roleName, [FromBody] List<string> permissions)
    {
        var ok = await _userProfileApiService.SaveRolePermissions(roleName, permissions);
        return ok ? Ok() : BadRequest();
    }

    [HttpPost]
    public async Task<IActionResult> SavePermissions(string selectedRole, List<string> permissions)
    {
        var ok = await _userProfileApiService.SaveRolePermissions(selectedRole, permissions ?? new());
        TempData[ok ? "Success" : "Error"] = ok
            ? "İzinler kaydedildi." : "İzinler kaydedilemedi.";
        return RedirectToAction(nameof(PendingUser), new { selectedRole });
    }

}
