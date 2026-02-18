using Microsoft.AspNetCore.Mvc;

namespace WebUILayer.ViewComponents.LayoutComponents;

public class LayoutPublicHeaderViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
