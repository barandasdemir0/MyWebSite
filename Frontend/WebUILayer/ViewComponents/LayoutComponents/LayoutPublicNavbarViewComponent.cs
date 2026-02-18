using Microsoft.AspNetCore.Mvc;

namespace WebUILayer.ViewComponents.LayoutComponents;

public class LayoutPublicNavbarViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
