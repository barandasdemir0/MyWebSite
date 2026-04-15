using Microsoft.AspNetCore.Mvc;

namespace WebUILayer.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
