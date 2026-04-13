using DtoLayer.SiteSettingDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using WebUILayer.Areas.Admin.Services.Abstract;
using WebUILayer.Extension;

namespace WebUILayer.Areas.Admin.Controllers;


[Area("Admin")]
[Authorize]
[Route("[area]/[controller]/[action]/{id?}")]
public class SiteSettingsController : Controller
{
    private readonly ISiteSettingsApiService _siteSettingsApiService;
    private readonly IMemoryCache _memoryCache;

    public SiteSettingsController(ISiteSettingsApiService siteSettingsApiService, IMemoryCache memoryCache)
    {
        _siteSettingsApiService = siteSettingsApiService;
        _memoryCache = memoryCache;
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
            _memoryCache.Remove("IsMaintenanceMode");
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddApiError(ex);
            return View(updateSiteSettingDto);
        }
    }




}
