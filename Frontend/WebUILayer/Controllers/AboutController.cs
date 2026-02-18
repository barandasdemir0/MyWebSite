using Microsoft.AspNetCore.Mvc;

namespace WebUILayer.Controllers;

public class AboutController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
