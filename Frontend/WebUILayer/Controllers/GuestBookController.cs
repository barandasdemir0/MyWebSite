using Microsoft.AspNetCore.Mvc;

namespace WebUILayer.Controllers;

public class GuestBookController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
