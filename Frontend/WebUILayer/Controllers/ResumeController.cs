using Microsoft.AspNetCore.Mvc;
using WebUILayer.Models;
using WebUILayer.Services.Abstract;

namespace WebUILayer.Controllers;

public class ResumeController : Controller
{
    private readonly IPublicSiteSettingsApiService _publicSiteSettingsApiService;
    private readonly IPublicCertificateApiService _publicCertificateApiService;
    private readonly IPublicEducationApiService _publicEducationApiService;
    private readonly IPublicExperienceApiService _publicExperienceApiService;

    public ResumeController(IPublicSiteSettingsApiService publicSiteSettingsApiService, IPublicCertificateApiService publicCertificateApiService, IPublicEducationApiService publicEducationApiService, IPublicExperienceApiService publicExperienceApiService)
    {
        _publicSiteSettingsApiService = publicSiteSettingsApiService;
        _publicCertificateApiService = publicCertificateApiService;
        _publicEducationApiService = publicEducationApiService;
        _publicExperienceApiService = publicExperienceApiService;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var siteSettings = await _publicSiteSettingsApiService.GetAllAsync();
            var models = new ResumeViewModel
            {
                certificateDtos = await _publicCertificateApiService.GetAllAsync(),
                educationDtos = await _publicEducationApiService.GetAllAsync(),
                experienceDtos = await _publicExperienceApiService.GetAllAsync(),
                siteSettingDto = siteSettings.FirstOrDefault()
            };
            return View(models);
        }
        catch (Exception)
        {
            // API Çökerse CV sayfası patlamadan boş başlasın
            return View(new ResumeViewModel());
        }
    }
}
