using CV.EntityLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Shared;
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

    public async Task<IActionResult> Index(PaginationQuery query)
    {
        var about = await _publicAboutApiService.GetAllAsync();
        var pagedResult = await _publicBlogPostApiService.GetAllPagedAsync(query);

        var models = new BlogViewModel
        {
            aboutDto = about.FirstOrDefault(),
            blogPostListDtos = pagedResult.Items,
            topicDtos = await _publicTopicApiService.GetAllAsync(),
            CurrentPage = pagedResult.PageNumber,
            TotalPages = pagedResult.TotalPages
        };

        return View(models);
    }

    public async Task<IActionResult> BlogDetail(string id)
    {
        
        if (string.IsNullOrEmpty(id))
        {
            return RedirectToAction(nameof(Index));
        }

        var blog = await _publicBlogPostApiService.GetBySlugAsync(id);
        if (blog==null)
        {
            return RedirectToAction(nameof(Index));
        }

        var about = await _publicAboutApiService.GetAllAsync();


        var relatedProjects = await _publicProjectApiService.GetLatestAsync(3, blog.MainTopic);

        var models = new BlogDetailViewModel
        {
            BlogPostDto = blog,
            AboutDto = about.FirstOrDefault(),
            SocialMediaDtos = await _publicSocialMediaApiService.GetAllAsync(),
            ProjectDtos = relatedProjects
        };
        return View(models);




    
    }
}
