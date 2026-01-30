using Microsoft.AspNetCore.Mvc;

namespace WebUILayer.Areas.Admin.ViewComponents.LayoutComponents
{
    public class _LayoutPreLoaderPartialComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
