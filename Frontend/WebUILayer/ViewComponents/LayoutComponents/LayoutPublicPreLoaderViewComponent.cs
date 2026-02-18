using Microsoft.AspNetCore.Mvc;

namespace WebUILayer.ViewComponents.LayoutComponents;

public class LayoutPublicPreLoaderViewComponent:ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
