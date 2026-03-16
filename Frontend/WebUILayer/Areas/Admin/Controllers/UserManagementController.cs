using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WebUILayer.Areas.Admin.Services.Abstract;

namespace WebUILayer.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
[Route("[area]/[controller]/[action]/{id?}")]
public class UserManagementController : Controller
{
    private readonly IUserProfileApiService _userProfileApiService;

    public UserManagementController(IUserProfileApiService userProfileApiService)
    {
        _userProfileApiService = userProfileApiService;
    }

    [HttpGet]
    public async Task<IActionResult> PendingUser()
    {
        var users = await _userProfileApiService.GetPendingUsersAsync();
        return View(users);
    }

    [HttpPost]
    public async Task<IActionResult> Approve(string id,string role="User")
    {
        var ok = await _userProfileApiService.ApproveUserAsync(id,role);
        TempData[ok ? "Success" : "Error"] = ok
            ? "Kullanıcı onaylandı." : "Kullanıcı onaylanamadı.";
        return RedirectToAction(nameof(PendingUser));
    }

    [HttpPost]
    public async Task<IActionResult> Reject(string id)
    {
        var ok = await _userProfileApiService.RejecetUserAsync(id);
        TempData[ok ? "Success" : "Error"] = ok ? "Kullanıcı reddedildi." : "Reddetme başarısız.";
        return RedirectToAction(nameof(PendingUser));

    }
}
