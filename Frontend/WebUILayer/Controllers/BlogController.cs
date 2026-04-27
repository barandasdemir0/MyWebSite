using Microsoft.AspNetCore.Mvc;
using WebUILayer.Models;
using WebUILayer.Services.Abstract;

namespace WebUILayer.Controllers;

public class BlogController : Controller
{

    private readonly IPublicBlogPostApiService _publicBlogPostApiService;
    private readonly IPublicTopicApiService _publicTopicApiService;
    private readonly IPublicProjectApiService _publicProjectApiService;
    private readonly IPublicAboutApiService _publicAboutApiService;
    private readonly IPublicSocialMediaApiService _publicSocialMediaApiService;

    public BlogController(IPublicBlogPostApiService publicBlogPostApiService, IPublicTopicApiService publicTopicApiService, IPublicProjectApiService publicProjectApiService, IPublicAboutApiService publicAboutApiService, IPublicSocialMediaApiService publicSocialMediaApiService)
    {
        _publicBlogPostApiService = publicBlogPostApiService;
        _publicTopicApiService = publicTopicApiService;
        _publicProjectApiService = publicProjectApiService;
        _publicAboutApiService = publicAboutApiService;
        _publicSocialMediaApiService = publicSocialMediaApiService;
    }

    public async Task<IActionResult> Index()
    {
        var about = await _publicAboutApiService.GetAllAsync();

        var models = new BlogViewModel
        {
            aboutDto = about.FirstOrDefault(),
            blogPostListDtos = await _publicBlogPostApiService.GetAllAsync(),
            topicDtos = await _publicTopicApiService.GetAllAsync(),
        };

        return View(models);
    }

    public async Task<IActionResult> BlogDetail()
    {
        var about = await _publicAboutApiService.GetAllAsync();
        var project = await _publicProjectApiService.GetLatestAsync(3);
        var models = new BlogViewModel
        {
            aboutDto = about.FirstOrDefault(),
            blogPostListDtos = await _publicBlogPostApiService.GetAllAsync(),
            topicDtos = await _publicTopicApiService.GetAllAsync(),
            socialMediaDtos = await _publicSocialMediaApiService.GetAllAsync(),
            projectListDtos = project
        };
        return View(models);
    }
}
