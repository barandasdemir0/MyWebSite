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
        try
        {
            
            var aboutsList = await _publicAboutApiService.GetAllAsync();
            var jobSkillsList = await _publicJobSkillApiService.GetAllAsync();
            var jobSkillCategoriesList = await _publicJobSkillCategoryService.GetAllAsync();
            var contactInfo = await _publicContactApiService.GetPublicContactSettingsAsync();
            var siteSettingsList = await _publicSiteSettingsApiService.GetAllAsync();
            var model = new AboutViewModel
            {
                About = aboutsList.FirstOrDefault(),
                jobSkillDtos = jobSkillsList,
                jobSkillCategoryDtos = jobSkillCategoriesList,
                Contact = contactInfo,
                SiteSetting = siteSettingsList.FirstOrDefault()
            };
            return View(model);
        }
        catch (Exception)
        {
            return View(new AboutViewModel());
        }
    }
}
