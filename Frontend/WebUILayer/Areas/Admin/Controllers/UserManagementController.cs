using CV.EntityLayer.Entities;
using DtoLayer.AuthDtos;
using DtoLayer.AuthDtos.Items;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebUILayer.Areas.Admin.Services.Abstract;

namespace WebUILayer.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
[Route("[area]/[controller]/[action]/{id?}")]
public class UserManagementController : Controller
{
    private readonly IUserAdminApiService _userAdminApiService;
    private readonly IRolePermissionApiService _rolePermissionApiService;


    public UserManagementController(IUserAdminApiService userAdminApiService, IRolePermissionApiService rolePermissionApiService)
    {
        _userAdminApiService = userAdminApiService;
        _rolePermissionApiService = rolePermissionApiService;
    }



    [HttpGet]
    public async Task<IActionResult> PendingUser(string selectedRole = RoleConsts.Editor) //string selectedRole = RoleConsts.Editor parametresi, onaylama sayfasında hangi rolün izinlerinin gösterileceğini belirler. Varsayılan olarak Editor rolü seçilmiştir.
    {
        var pending = await _userAdminApiService.GetPendingUsersAsync(); // Bekleyen kullanıcıları al
        var approved = await _userAdminApiService.GetAllUsersAsync(); // Onaylanmış kullanıcıları al
        var perms = await _rolePermissionApiService.GetRolePermissionsAsync(selectedRole); // Seçilen rolün izinlerini al
        var vm = new UserManagementViewModel
        {
            PendingUsers = pending, // Bekleyen kullanıcıları view modeline ata
            ApprovedUsers = approved,// Onaylanmış kullanıcıları view modeline ata
            SelectedRole = selectedRole, // Seçilen rolü view modeline ata
            ActivePermissions = perms, // Seçilen rolün aktif izinlerini view modeline ata
            AllPermissions = PermissionConsts.GetAll().Select(p => new PermissionItem // Sistemdeki tüm izinleri temsil eden bir liste oluşturulur ve bu liste view modeline atanır. Bu sayede onaylama sayfasında hangi izinlerin mevcut olduğunu göstermek mümkün olur.
            {
                Key = p.Key,
                Label = p.Label
            }).ToList(), // Tüm izinleri view modeline ata
            AllRoles = new List<string> //sistemdeki tüm rolleri temsil eden bir liste oluşturulur ve bu liste view modeline atanır. Bu sayede onaylama sayfasında hangi rollerin mevcut olduğunu göstermek mümkün olur.
            {
                RoleConsts.User,
                RoleConsts.Editor,
                RoleConsts.Admin
            }, // Tüm rolleri view modeline ata
            SelectableRoles = new List<string> // Onaylama sırasında seçilebilecek rolleri temsil eden bir liste oluşturulur ve bu liste view modeline atanır. Bu sayede onaylama sırasında hangi rollerin seçilebileceği belirlenmiş olur.
            {
                RoleConsts.Editor,
                RoleConsts.User
            }// Onaylama sırasında seçilebilecek rolleri view modeline ata
        };
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Approve(string id, string role = RoleConsts.User)
    {
        var ok = await _userAdminApiService.ApproveUserAsync(id, role); // Kullanıcıyı onayla ve rolünü ata
        TempData[ok ? "Success" : "Error"] = ok
            ? "Kullanıcı onaylandı." : "Kullanıcı onaylanamadı.";
        return RedirectToAction(nameof(PendingUser)); // Onaylama işleminden sonra bekleyen kullanıcılar sayfasına yönlendir
    }

    [HttpPost]
    public async Task<IActionResult> Reject(string id) // Kullanıcıyı reddet
    {
        var ok = await _userAdminApiService.RejectUserAsync(id);
        TempData[ok ? "Success" : "Error"] = ok ? "Kullanıcı reddedildi." : "Reddetme başarısız.";
        return RedirectToAction(nameof(PendingUser));

    }

    [HttpGet]
    public async Task<IActionResult> ManageUsers() // Tüm kullanıcıları yönet sayfası
    {
        var users = await _userAdminApiService.GetAllUsersAsync();
        return View(users);
    }
    [HttpGet]// kısa anlat 3 kelime : rol izinlerini getir
    public async Task<IActionResult> GetRolePermissions(string roleName)
    {
        var perms = await _rolePermissionApiService.GetRolePermissionsAsync(roleName);
        return Json(perms);
    }
    [HttpPost] // kısa anlat 3 kelime : rol izinlerini kaydet
    public async Task<IActionResult> SaveRolePermissions(
        string roleName, [FromBody] List<string> permissions) //burası : SaveRolePermissions metodu, bir rolün izinlerini kaydetmek için kullanılır. roleName parametresi, izinlerin hangi role ait olduğunu belirtirken, permissions parametresi ise kaydedilecek izinlerin listesini içerir. [FromBody] attribute'u, bu listenin HTTP isteğinin gövdesinden alınacağını belirtir. Bu sayede, istemciden gelen JSON formatındaki izin listesi doğru şekilde işlenebilir ve kaydedilebilir.
    {
        var ok = await _rolePermissionApiService.SaveRolePermissions(roleName, permissions);
        if (ok)
        {
            return Ok();
        }
        else
        {
            return BadRequest();
        }
    }


    // İzinleri kaydet
    [HttpPost]
    public async Task<IActionResult> SavePermissions(string selectedRole, List<string> permissions)//seçilen rolldeki izni listeye al kaydet.
    {
        var ok = await _rolePermissionApiService.SaveRolePermissions(selectedRole, permissions ?? new());
        TempData[ok ? "Success" : "Error"] = ok
            ? "İzinler kaydedildi." : "İzinler kaydedilemedi.";
        return RedirectToAction(nameof(PendingUser), new { selectedRole }); // İzin kaydetme işleminden sonra bekleyen kullanıcılar sayfasına yönlendir, seçilen rolü de koru
    }

}
