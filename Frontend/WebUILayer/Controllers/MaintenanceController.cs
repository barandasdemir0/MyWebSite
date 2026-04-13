using Microsoft.AspNetCore.Mvc;
using WebUILayer.Areas.Admin.Services.Concrete;
using WebUILayer.Models;

namespace WebUILayer.Controllers;

public class MaintenanceController : Controller
{
    private readonly SiteSettingsApiService _siteSettingsApiService;
    private readonly ContactApiService _contactApiService;

    public MaintenanceController(SiteSettingsApiService siteSettingsApiService, ContactApiService contactApiService)
    {
        _siteSettingsApiService = siteSettingsApiService;
        _contactApiService = contactApiService;
    }


    [HttpGet("/maintenance")]
    public async Task<IActionResult> Index()
    {
        var settings = await _siteSettingsApiService.GetSiteSettingForEditAsync();

        var contact = await _contactApiService.GetContactForEditAsync();

        //var model = (Settings: settings, Contact: contact);

        var model = new MaintenanceViewModel
        {
            contactDtos = contact,
            siteSettingDtos = settings
            
        };
        return View(model);
    }
}
