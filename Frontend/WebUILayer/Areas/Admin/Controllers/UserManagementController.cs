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
    public async Task<IActionResult> Approve(string id)
    {
        var ok = await _userProfileApiService.ApproveUserAsync(id);
        TempData[ok ? "Success" : "Error"] = ok
            ? "Kullanıcı onaylandı." : "Kullanıcı onaylanamadı.";
        return RedirectToAction(nameof(PendingUser));
    }
}
