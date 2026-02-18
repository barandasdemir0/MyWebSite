using Microsoft.AspNetCore.Mvc;

namespace WebUILayer.ViewComponents.LayoutComponents;

public class LayoutPublicScriptsViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
