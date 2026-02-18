using Microsoft.AspNetCore.Mvc;

namespace WebUILayer.Controllers;

public class ProjectController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
    public IActionResult ProjectDetail()
    {
        return View();
    }
}
