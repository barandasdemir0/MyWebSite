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
            // İstekleri çakışmaması için sırayla çekiyoruz
            var aboutList = await _publicAboutApiService.GetAllAsync();
            var siteSettingsList = await _publicSiteSettingsApiService.GetAllAsync();
            var heroList = await _publicHeroApiService.GetAllAsync();
            var socialMediaList = await _publicSocialMediaApiService.GetAllAsync();
            var skillList = await _publicSkillApiService.GetAllAsync();
            var projectList = await _publicProjectApiService.GetLatestAsync(3);
            var guestBookList = await _publicGuestBookApiService.GetAllAsync();
            var githubList = await _publicGithubApiService.GetAllAsync();
            var models = new IndexViewModel
            {
                aboutDto = aboutList.FirstOrDefault(),
                siteSettingDto = siteSettingsList.FirstOrDefault(),
                heroDto = heroList.FirstOrDefault(),
                socialMediaDtos = socialMediaList,
                skillDtos = skillList,
                projectListDtos = projectList,
                guestBookListDtos = guestBookList,
                githubRepoDtos = githubList
            };
            return View(models);
        }
        catch (Exception)
        {
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
