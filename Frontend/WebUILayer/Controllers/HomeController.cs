using Microsoft.AspNetCore.Mvc;
using WebUILayer.Models;
using WebUILayer.Services.Abstract;

namespace WebUILayer.Controllers;

public class HomeController : Controller
{
    private readonly IPublicSiteSettingsApiService _publicSiteSettingsApiService;
    private readonly IPublicHeroApiService _publicHeroApiService;
    private readonly IPublicSocialMediaApiService _publicSocialMediaApiService;
    private readonly IPublicAboutApiService _publicAboutApiService;
    private readonly IPublicSkillApiService _publicSkillApiService;
    private readonly IPublicProjectApiService _publicProjectApiService;
    private readonly IPublicGuestBookApiService _publicGuestBookApiService;
    private readonly IPublicGithubApiService _publicGithubApiService;

    public HomeController(IPublicSiteSettingsApiService publicSiteSettingsApiService, IPublicHeroApiService publicHeroApiService, IPublicSocialMediaApiService publicSocialMediaApiService, IPublicAboutApiService publicAboutApiService, IPublicSkillApiService publicSkillApiService, IPublicProjectApiService publicProjectApiService, IPublicGuestBookApiService publicGuestBookApiService, IPublicGithubApiService publicGithubApiService)
    {
        _publicSiteSettingsApiService = publicSiteSettingsApiService;
        _publicHeroApiService = publicHeroApiService;
        _publicSocialMediaApiService = publicSocialMediaApiService;
        _publicAboutApiService = publicAboutApiService;
        _publicSkillApiService = publicSkillApiService;
        _publicProjectApiService = publicProjectApiService;
        _publicGuestBookApiService = publicGuestBookApiService;
        _publicGithubApiService = publicGithubApiService;
    }

    public async Task<IActionResult> Index()
    {
        var about = await _publicAboutApiService.GetAllAsync();
        var siteSettings = await _publicSiteSettingsApiService.GetAllAsync();
        var hero = await _publicHeroApiService.GetAllAsync();
        var socialMedia = await _publicSocialMediaApiService.GetAllAsync();
        var skill = await _publicSkillApiService.GetAllAsync();
        var project = await _publicProjectApiService.GetLatestAsync(3);
        var guestBook = await _publicGuestBookApiService.GetAllAsync();
        var github = await _publicGithubApiService.GetAllAsync();

        var models = new IndexViewModel
        {
            aboutDto = about.FirstOrDefault(),
            siteSettingDto = siteSettings.FirstOrDefault(),
            heroDto = hero.FirstOrDefault(),
            socialMediaDtos = socialMedia,
            skillDtos = skill,
            projectListDtos = project,
            guestBookListDtos = guestBook,
            githubRepoDtos = github
        };


        return View(models);
    }
}
