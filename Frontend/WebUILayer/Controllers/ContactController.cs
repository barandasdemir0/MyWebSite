using Microsoft.AspNetCore.Mvc;

namespace WebUILayer.Controllers;

public class ContactController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
