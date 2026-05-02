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
        var aboutTask = _publicAboutApiService.GetAllAsync();
        var siteSettingsTask = _publicSiteSettingsApiService.GetAllAsync();
        var heroTask = _publicHeroApiService.GetAllAsync();
        var socialMediaTask = _publicSocialMediaApiService.GetAllAsync();
        var skillTask = _publicSkillApiService.GetAllAsync();
        var projectTask = _publicProjectApiService.GetLatestAsync(3);
        var guestBookTask = _publicGuestBookApiService.GetAllAsync();
        var githubTask = _publicGithubApiService.GetAllAsync();

        await Task.WhenAll(
        aboutTask, siteSettingsTask, heroTask, socialMediaTask,
        skillTask, projectTask, guestBookTask, githubTask
    );
        var models = new IndexViewModel
        {
            aboutDto = aboutTask.Result.FirstOrDefault(),
            siteSettingDto = siteSettingsTask.Result.FirstOrDefault(),
            heroDto = heroTask.Result.FirstOrDefault(),
            socialMediaDtos = socialMediaTask.Result,
            skillDtos = skillTask.Result,
            projectListDtos = projectTask.Result,
            guestBookListDtos = guestBookTask.Result,
            githubRepoDtos = githubTask.Result
        };
        return View(models);
    }
}
