using Microsoft.AspNetCore.Mvc;
using WebUILayer.Areas.Admin.Services.Abstract;

namespace WebUILayer.Areas.Admin.ViewComponents.LayoutComponents;

public class _LayoutNavbarPartialComponent:ViewComponent
{
    private readonly INotificationApiService _notificationApiService;

    public _LayoutNavbarPartialComponent(INotificationApiService notificationApiService)
    {
        _notificationApiService = notificationApiService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var unreadNotification = await _notificationApiService.GetTopUnreadAsync(5);
        return View(unreadNotification);
    }
}
