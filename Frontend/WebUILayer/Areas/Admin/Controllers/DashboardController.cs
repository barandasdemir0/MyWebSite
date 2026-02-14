using Microsoft.AspNetCore.Mvc;
using WebUILayer.Areas.Admin.Models;
using WebUILayer.Areas.Admin.Services.Abstract;

namespace WebUILayer.Areas.Admin.Controllers;

[Area("Admin")]
[Route("[area]/[controller]/[action]/{id?}")]
public class DashboardController : Controller
{
    private readonly IBlogPostApiService _blogPostApiService;
    private readonly IProjectApiService _projectApiService;

    public DashboardController(IBlogPostApiService blogPostApiService, IProjectApiService projectApiService)
    {
        _blogPostApiService = blogPostApiService;
        _projectApiService = projectApiService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
       
        var model = new DashboardIndexViewModel
        {
            blogPostListDtos = await _blogPostApiService.GetLatestAsync(3), //3 tanesini getir diyoruz
            projectListDtos = await _projectApiService.GetAllAsync(),
          
           
        };
        return View(model);
    }
}
