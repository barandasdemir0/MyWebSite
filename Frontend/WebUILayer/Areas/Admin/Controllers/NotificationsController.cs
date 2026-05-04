using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Shared;
using WebUILayer.Areas.Admin.Models;
using WebUILayer.Areas.Admin.Services.Abstract;
using WebUILayer.Extension;

namespace WebUILayer.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
[Route("[area]/[controller]/[action]/{id?}")]
public class NotificationsController : Controller
{

    private readonly INotificationApiService _notificationApiService;

    public NotificationsController(INotificationApiService notificationApiService)
    {
        _notificationApiService = notificationApiService;
    }

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] PaginationQuery paginationQuery)
    {
        var pagedResult = await _notificationApiService.GetAllAdminAsync(paginationQuery);

        var model = new NotificationIndexViewModel
        {
            NotificationDtos = pagedResult.Items,
            CurrentPage = pagedResult.PageNumber,
            TotalPages = pagedResult.TotalPages
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Read(Guid id)
    {
        await _notificationApiService.ReadMessageAsync(id);
        string referer = Request.Headers["Referer"].ToString();
        return Redirect(string.IsNullOrEmpty(referer) ? "/Admin/Dashboard/Index" : referer);
    }


    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {

        return await this.SafeAction
            (
            action: () => _notificationApiService.DeleteAsync(id),
            successMessage: "Silme işlemi Başarılı oldu",
            ErrorMessage: "Silme İşlemi Başarısız oldu"
            );
    }
    [HttpPost]
    public async Task<IActionResult> Restore(Guid id)
    {

        return await this.SafeAction
            (
            action: () => _notificationApiService.RestoreAsync(id),
            successMessage: "Geri Alma işlemi Başarılı oldu",
            ErrorMessage: "Geri Alma İşlemi Başarısız oldu"
            );
    }

}
