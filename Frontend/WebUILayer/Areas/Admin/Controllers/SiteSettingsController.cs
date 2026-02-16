using DtoLayer.SiteSettingDtos;
using Microsoft.AspNetCore.Mvc;
using WebUILayer.Areas.Admin.Services.Abstract;
using WebUILayer.Extension;

namespace WebUILayer.Areas.Admin.Controllers;


[Area("Admin")]
[Route("[area]/[controller]/[action]/{id?}")]
public class SiteSettingsController : Controller
{
    private readonly ISiteSettingsApiService _siteSettingsApiService;

    public SiteSettingsController(ISiteSettingsApiService siteSettingsApiService)
    {
        _siteSettingsApiService = siteSettingsApiService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var query = await _siteSettingsApiService.GetSiteSettingForEditAsync();
        return View(query);
    }

    [HttpPost]
    public async Task<IActionResult> Index(UpdateSiteSettingDto updateSiteSettingDto)
    {
        if (!ModelState.IsValid)
        {
            return View(updateSiteSettingDto);
        }
        try
        {
            await _siteSettingsApiService.SaveAboutAsync(updateSiteSettingDto);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddApiError(ex);
            return View(updateSiteSettingDto);
        }
    }




}
