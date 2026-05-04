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
            // 1. Bağımsız verilerin HEPSİNİ AYNI ANDA başlatıyoruz (Performans artışı)
            var aboutTask = _publicAboutApiService.GetAllAsync();
            var pagedResultTask = _publicBlogPostApiService.GetAllPagedAsync(query);
            var topicTask = _publicTopicApiService.GetAllAsync();
            // 2. İşlemlerin bitmesini paralel olarak bekliyoruz
            await Task.WhenAll(aboutTask, pagedResultTask, topicTask);
            var models = new BlogViewModel
            {
                aboutDto = aboutTask.Result.FirstOrDefault(),
                blogPostListDtos = pagedResultTask.Result.Items,
                topicDtos = topicTask.Result,
                CurrentPage = pagedResultTask.Result.PageNumber,
                TotalPages = pagedResultTask.Result.TotalPages
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
            var aboutTask = _publicAboutApiService.GetAllAsync();
            var relatedProjectsTask = _publicProjectApiService.GetLatestAsync(3, blog.MainTopic);
            var socialMediaTask = _publicSocialMediaApiService.GetAllAsync();
            // Üçünü aynı anda paralel bekle (Sayfa açılışı 3 kat hızlanır)
            await Task.WhenAll(aboutTask, relatedProjectsTask, socialMediaTask);
            var models = new BlogDetailViewModel
            {
                BlogPostDto = blog,
                AboutDto = aboutTask.Result.FirstOrDefault(),
                SocialMediaDtos = socialMediaTask.Result,
                ProjectDtos = relatedProjectsTask.Result
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
