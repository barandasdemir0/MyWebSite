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
        try
        {
            var aboutList = await _publicAboutApiService.GetAllAsync();
            var pagedResult = await _publicBlogPostApiService.GetAllPagedAsync(query);
            var topics = await _publicTopicApiService.GetAllAsync();

            var models = new BlogViewModel
            {
                aboutDto = aboutList.FirstOrDefault(),
                blogPostListDtos = pagedResult.Items,
                topicDtos = topics,
                CurrentPage = pagedResult.PageNumber,
                TotalPages = pagedResult.TotalPages
            };
            return View(models);
        }
        catch (Exception)
        {
            // Eğer API çökmüşse veya veritabanı bağlantısı koptuysa anasayfaya at
            TempData["Error"] = "Blog yazıları şu anda yüklenemiyor, lütfen daha sonra tekrar deneyin.";
            // Not: Eğer HomeController veya DefaultController varsa oraya yönlendirebilirsin
            return RedirectToAction("Index", "Default");
        }
    }

    public async Task<IActionResult> BlogDetail(string id)
    {

        if (string.IsNullOrEmpty(id))
        {
            return RedirectToAction(nameof(Index));
        }
        try
        {
            // 1. AŞAMA: Önce Blog'u çekmek ZORUNDAYIZ (Çünkü kategorisi 'MainTopic' lazım)
            var blog = await _publicBlogPostApiService.GetBySlugAsync(id);
            if (blog == null)
            {
                return RedirectToAction(nameof(Index));
            }
            // 2. AŞAMA: Blog geldiğine göre geri kalan her şeyi AYNI ANDA çekebiliriz
            var aboutList = await _publicAboutApiService.GetAllAsync();
            var relatedProjects = await _publicProjectApiService.GetLatestAsync(3, blog.MainTopic);
            var socialMediaList = await _publicSocialMediaApiService.GetAllAsync();

            var models = new BlogDetailViewModel
            {
                BlogPostDto = blog,
                AboutDto = aboutList.FirstOrDefault(),
                SocialMediaDtos = socialMediaList,
                ProjectDtos = relatedProjects
            };

            return View(models);
        }
        catch (Exception)
        {
            TempData["Error"] = "Bu blog yazısına şu anda ulaşılamıyor.";
            return RedirectToAction(nameof(Index));
        }



    }
}
