using Microsoft.AspNetCore.Mvc;

namespace WebUILayer.Controllers;

public class IndexController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
