using Microsoft.AspNetCore.Mvc;

namespace WebUILayer.ViewComponents.LayoutComponents;

public class LayoutPublicFooterViewComponent:ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
