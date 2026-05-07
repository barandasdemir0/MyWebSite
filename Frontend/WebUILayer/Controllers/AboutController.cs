using Microsoft.AspNetCore.Mvc;
using WebUILayer.Areas.Admin.Services.Abstract;
using WebUILayer.Models;
using WebUILayer.Services.Abstract;

namespace WebUILayer.Controllers;

public class AboutController : Controller
{

    private readonly IPublicAboutApiService _publicAboutApiService;
    private readonly IPublicJobSkillApiService _publicJobSkillApiService;
    private readonly IPublicJobSkillCategoryService _publicJobSkillCategoryService;
    private readonly IPublicContactApiService _publicContactApiService;
    private readonly IPublicSiteSettingsApiService _publicSiteSettingsApiService;

    public AboutController(IPublicAboutApiService publicAboutApiService, IPublicJobSkillApiService publicJobSkillApiService, IPublicJobSkillCategoryService publicJobSkillCategoryService, IPublicContactApiService publicContactApiService, IPublicSiteSettingsApiService publicSiteSettingsApiService)
    {
        _publicAboutApiService = publicAboutApiService;
        _publicJobSkillApiService = publicJobSkillApiService;
        _publicJobSkillCategoryService = publicJobSkillCategoryService;
        _publicContactApiService = publicContactApiService;
        _publicSiteSettingsApiService = publicSiteSettingsApiService;
    }

    public async Task<IActionResult> Index()
    {
        // 1. Tüm istekleri "await" kullanmadan AYNI ANDA başlat
        var aboutsTask = _publicAboutApiService.GetAllAsync();
        var jobSkillsTask = _publicJobSkillApiService.GetAllAsync();
        var jobSkillCategoriesTask = _publicJobSkillCategoryService.GetAllAsync();
        var contactTask = _publicContactApiService.GetPublicContactSettingsAsync();
        var siteSettingsTask = _publicSiteSettingsApiService.GetAllAsync();
        // 2. Bütün görevlerin bitmesini paralel olarak bekle
        await Task.WhenAll(aboutsTask, jobSkillsTask, jobSkillCategoriesTask, contactTask, siteSettingsTask);
        // 3. Gelen sonuçları (Result) modele aktar
        var model = new AboutViewModel
        {
            About = aboutsTask.Result.FirstOrDefault(),
            jobSkillDtos = jobSkillsTask.Result,
            jobSkillCategoryDtos = jobSkillCategoriesTask.Result,
            Contact = contactTask.Result,
            SiteSetting = siteSettingsTask.Result.FirstOrDefault()
        };
        return View(model);
    }
}
