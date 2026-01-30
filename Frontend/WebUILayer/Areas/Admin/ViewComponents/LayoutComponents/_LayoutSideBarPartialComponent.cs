using Microsoft.AspNetCore.Mvc;

namespace WebUILayer.Areas.Admin.ViewComponents.LayoutComponents
{
    public class _LayoutSideBarPartialComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
