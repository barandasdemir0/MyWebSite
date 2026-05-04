using Microsoft.AspNetCore.Mvc;
using WebUILayer.Areas.Admin.Services.Abstract;

namespace WebUILayer.ViewComponents.LayoutComponents;

public class LayoutPublicNavbarViewComponent : ViewComponent
{

    public IViewComponentResult Invoke()
    {

        return View();
    }
}
