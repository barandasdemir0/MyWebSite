using DtoLayer.BlogPostDtos;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Shared;
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

    public async Task<IActionResult> Index([FromQuery] PaginationQuery query)
    {
        try
        {
            var pagedResult = await _publicProjectApiService.GetAllPagedAsync(query);
            var models = new ProjectViewModel
            {
                topicDtos = await _publicTopicApiService.GetAllAsync(),
                projectDtos = await _publicProjectApiService.GetAllAsync(),
                CurrentPage = pagedResult.PageNumber,
                TotalPages = pagedResult.TotalPages
            };
            return View(models);
        }
        catch (Exception)
        {
            // API çökerse sayfa boş açılsın ama 500 fırlatmasın
            return View(new ProjectViewModel());
        }
    }
    public async Task<IActionResult> ProjectDetail(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return RedirectToAction(nameof(Index));
        }
        try
        {
            var project = await _publicProjectApiService.GetBySlugAsync(id);
            if (project == null)
            {
                return RedirectToAction(nameof(Index));
            }
           

            string? mainTopic = null;
            if (project.Topics!=null)
            {
                mainTopic = project.Topics.FirstOrDefault();
            }

            List<BlogPostDto> relatedBlogs = new();

            // 2. KISIM: mainTopic doluysa API'ye gidiyoruz, boşsa hiçbir şey yapmıyoruz (null kalıyor)
            if (mainTopic != null)
            {
                relatedBlogs = await _publicBlogPostApiService.GetLatestAsync(3, mainTopic);
            }

            var models = new ProjectViewModel
            {
                ProjectDto = project,
                blogPostDtos = relatedBlogs
            };

            return View(models);
        }
        catch (Exception)
        {
            return RedirectToAction(nameof(Index));
        }
    }
}
