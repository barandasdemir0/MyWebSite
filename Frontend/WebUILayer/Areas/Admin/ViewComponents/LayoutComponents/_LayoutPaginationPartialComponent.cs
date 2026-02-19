using Microsoft.AspNetCore.Mvc;
using WebUILayer.Areas.Admin.Models;

namespace WebUILayer.Areas.Admin.ViewComponents.LayoutComponents;


public class _LayoutPaginationPartialComponent : ViewComponent
{
    public IViewComponentResult Invoke(int currentPage, int totalPages, string action = "Index",
    Dictionary<string, string>? extraRouteValues = null)
    {
        return View(new PaginationViewModel
        {
            CurrentPage = currentPage,
            TotalPages = totalPages,
            Action = action,
            ExtraRouteValues = extraRouteValues ?? new()
        });
    }
}
