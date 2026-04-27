using Microsoft.AspNetCore.Mvc;
using WebUILayer.Models;
using WebUILayer.Services.Abstract;

namespace WebUILayer.Controllers;

public class ProjectController : Controller
{
    private readonly IPublicTopicApiService _publicTopicApiService;
    private readonly IPublicBlogPostApiService _publicBlogPostApiService;
    private readonly IPublicProjectApiService _publicProjectApiService;

    public ProjectController(IPublicTopicApiService publicTopicApiService, IPublicBlogPostApiService publicBlogPostApiService, IPublicProjectApiService publicProjectApiService)
    {
        _publicTopicApiService = publicTopicApiService;
        _publicBlogPostApiService = publicBlogPostApiService;
        _publicProjectApiService = publicProjectApiService;
    }

    public async Task<IActionResult> Index()
    {
        var models = new ProjectViewModel
        {
            topicDtos = await _publicTopicApiService.GetAllAsync(),
            projectDtos = await _publicProjectApiService.GetAllAsync()
        };

        return View(models);
    }
    public async Task<IActionResult> ProjectDetail()
    {
        var blogs = await _publicBlogPostApiService.GetLatestAsync(3);

        var models = new ProjectViewModel
        {
            topicDtos = await _publicTopicApiService.GetAllAsync(),
            projectDtos = await _publicProjectApiService.GetAllAsync()
        };

        return View(models);
    }
}
