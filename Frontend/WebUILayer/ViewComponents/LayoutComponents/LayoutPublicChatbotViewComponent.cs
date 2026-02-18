using Microsoft.AspNetCore.Mvc;

namespace WebUILayer.ViewComponents.LayoutComponents;

public class LayoutPublicChatbotViewComponent:ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
