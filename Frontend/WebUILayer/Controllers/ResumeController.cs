using Microsoft.AspNetCore.Mvc;

namespace WebUILayer.Controllers;

public class ResumeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
