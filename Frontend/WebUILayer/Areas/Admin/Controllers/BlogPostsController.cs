using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebUILayer.Areas.Admin.Services.Abstract;

namespace WebUILayer.Areas.Admin.Controllers;

[Area("Admin")]
[Route("[area]/[controller]/[action]/{id?}")]
public class BlogPostsController : Controller
{
    private readonly IBlogPostApiService _blogPostApiService;

    public BlogPostsController(IBlogPostApiService blogPostApiService)
    {
        _blogPostApiService = blogPostApiService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var query = await _blogPostApiService.GetAllAdminAsync();

        return View(query);
    }
}
