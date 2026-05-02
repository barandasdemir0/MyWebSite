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
        var abouts = await _publicAboutApiService.GetAllAsync();


        var jobSkills = await _publicJobSkillApiService.GetAllAsync();
        var jobSkillCategories = await _publicJobSkillCategoryService.GetAllAsync();
        var contact = await _publicContactApiService.GetAllAsync();
        var siteSettings = await _publicSiteSettingsApiService.GetAllAsync();


        var model = new AboutViewModel
        {
            About = abouts.FirstOrDefault(),
            jobSkillDtos = jobSkills,
            jobSkillCategoryDtos = jobSkillCategories,
            Contact = contact.FirstOrDefault(),
            SiteSetting = siteSettings.FirstOrDefault()

        };


        return View(model);
    }
}
