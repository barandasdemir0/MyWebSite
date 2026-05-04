using Microsoft.AspNetCore.Mvc;
using WebUILayer.Areas.Admin.Services.Abstract;
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
    private readonly ISiteSettingsApiService _siteSettingsApiService;

    public HomeController(IPublicSiteSettingsApiService publicSiteSettingsApiService, IPublicHeroApiService publicHeroApiService, IPublicSocialMediaApiService publicSocialMediaApiService, IPublicAboutApiService publicAboutApiService, IPublicSkillApiService publicSkillApiService, IPublicProjectApiService publicProjectApiService, IPublicGuestBookApiService publicGuestBookApiService, IPublicGithubApiService publicGithubApiService, ISiteSettingsApiService siteSettingsApiService)
    {
        _publicSiteSettingsApiService = publicSiteSettingsApiService;
        _publicHeroApiService = publicHeroApiService;
        _publicSocialMediaApiService = publicSocialMediaApiService;
        _publicAboutApiService = publicAboutApiService;
        _publicSkillApiService = publicSkillApiService;
        _publicProjectApiService = publicProjectApiService;
        _publicGuestBookApiService = publicGuestBookApiService;
        _publicGithubApiService = publicGithubApiService;
        _siteSettingsApiService = siteSettingsApiService;
    }

    public async Task<IActionResult> Index()
    {
        try
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
        catch (Exception)
        {
            // API çökerse anasayfa 500 hatası vermesin, View'a boş bir nesne dönsün
            return View(new IndexViewModel());
        }
    }


    [HttpGet]
    public async Task<IActionResult> DownloadCv()
    {
        // Dili cookie'den oku
        var googtrans = Request.Cookies["googtrans"] ?? "";
        var isEnglish = googtrans.Contains("/en");
        // SiteSettings'ten CV URL'sini al
        var settings = await _siteSettingsApiService.GetSiteSettingForEditAsync();
        var cvUrl = isEnglish
            ? (settings?.CvFileUrlEn ?? settings?.CvFileUrlTr ?? "/")
            : (settings?.CvFileUrlTr ?? settings?.CvFileUrlEn ?? "/");
        if (string.IsNullOrEmpty(cvUrl) || cvUrl == "/")
            return NotFound();
        // Dosyayı sunucu üzerinden çekip kullanıcıya aktar
        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
        var fileBytes = await httpClient.GetByteArrayAsync(cvUrl);

        var fileName = isEnglish ? "Baran_Dasdemir_CV_EN.pdf" : "Baran_Dasdemir_CV_TR.pdf";
        return File(fileBytes, "application/pdf", fileName);
    }

}
