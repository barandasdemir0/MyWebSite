using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Shared;
using WebUILayer.Areas.Admin.Models;
using WebUILayer.Areas.Admin.Services.Abstract;

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
            TotalPages = pagedResult.PageSize
        };
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Read(Guid id)
    {
        await _notificationApiService.ReadMessageAsync(id);
        string referer = Request.Headers["Referer"].ToString();
        return Redirect(string.IsNullOrEmpty(referer) ? "/Admin/Dashboard/Index" : referer);
    }

}
