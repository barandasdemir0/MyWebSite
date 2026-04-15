using Microsoft.AspNetCore.Mvc;
using WebUILayer.Areas.Admin.Services.Abstract;
using WebUILayer.Areas.Admin.Services.Concrete;
using WebUILayer.Models;

namespace WebUILayer.Controllers;

public class MaintenanceController : Controller
{
    private readonly ISiteSettingsApiService _siteSettingsApiService;
    private readonly IContactApiService _contactApiService;

    public MaintenanceController(ISiteSettingsApiService siteSettingsApiService, IContactApiService contactApiService)
    {
        _siteSettingsApiService = siteSettingsApiService;
        _contactApiService = contactApiService;
    }

    [HttpGet("/maintenance")]
    public async Task<IActionResult> Index()
    {
        var settings = await _siteSettingsApiService.GetSiteSettingForEditAsync();
        if (settings == null||!settings.IsMaintenanceMode)
        {
            return RedirectToAction("Index", "Home");
        }

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
