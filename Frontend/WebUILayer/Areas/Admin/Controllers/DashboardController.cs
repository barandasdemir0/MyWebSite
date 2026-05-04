using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebUILayer.Areas.Admin.Models;
using WebUILayer.Areas.Admin.Services.Abstract;

namespace WebUILayer.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
[Route("[area]/[controller]/[action]/{id?}")]
public class DashboardController : Controller
{
    private readonly IBlogPostApiService _blogPostApiService;
    private readonly IProjectApiService _projectApiService;
    private readonly IGuestBookApiService _guestBookApiService;
    private readonly IMessageApiService _messageApiService;

    public DashboardController(IBlogPostApiService blogPostApiService, IProjectApiService projectApiService, IGuestBookApiService guestBookApiService, IMessageApiService messageApiService)
    {
        _blogPostApiService = blogPostApiService;
        _projectApiService = projectApiService;
        _guestBookApiService = guestBookApiService;
        _messageApiService = messageApiService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        // AWAIT YAZMADAN Görevleri (Task) başlatıp eşzamanlı olarak API'lere koşturuyoruz
        var blogsTask = _blogPostApiService.GetLatestAsync(3);
        var projectsTask = _projectApiService.GetLatestAsync(3);
        var guestBookTask = _guestBookApiService.GetLatestAsync(3);
        var messagesTask = _messageApiService.GetLatestAsync(3);
        // Bütün görevlerin (Aynı anda) bitmesini bekle!
        await Task.WhenAll(blogsTask, projectsTask, guestBookTask, messagesTask);
        var model = new DashboardIndexViewModel
        {
            // Görevler bitti, sonuçları (Result) direkt al
            blogPostListDtos = await blogsTask,
            projectListDtos = await projectsTask,
            guestBookListDtos = await guestBookTask,
            messageListDtos = await messagesTask
        };

        return View(model);
    }
}
