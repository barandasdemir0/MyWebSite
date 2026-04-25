using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Shared;
using WebUILayer.Areas.Admin.Services.Abstract;
using WebUILayer.Extension;

namespace WebUILayer.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
[Route("[area]/[controller]/[action]/{id?}")]
public class GuestBooksController : Controller
{
    private readonly IGuestBookApiService _guestBookApiService;

    public GuestBooksController(IGuestBookApiService guestBookApiService)
    {
        _guestBookApiService = guestBookApiService;
    }
    

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] PaginationQuery paginationQuery)
    {
        var pagedResult = await _guestBookApiService.GetAllAdminAsync(paginationQuery);
        return View(pagedResult);
    }

    [HttpPost]
    public async Task<IActionResult> Approve(Guid id)
    {
        return await this.SafeAction(
                action: () => _guestBookApiService.ApproveAsync(id),
                successMessage: "Mesaj başarıyla onaylandı ve yayına alındı.",
                ErrorMessage: "Mesaj onaylanırken bir hata oluştu."
            );
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        return await this.SafeAction(
                action: () => _guestBookApiService.DeleteAsync(id),
                successMessage: "Mesaj başarıyla silindi.",
                ErrorMessage: "Mesaj silinirken bir hata oluştu."
            );
    }

    [HttpPost]
    public async Task<IActionResult> Restore(Guid id)
    {
        return await this.SafeAction(
            action: () => _guestBookApiService.RestoreAsync(id),
            successMessage: "Mesaj başarıyla geri yüklendi.",
            ErrorMessage: "Mesaj geri yüklenirken bir hata oluştu."
        );
    }









}
