using DtoLayer.ContactDtos;
using DtoLayer.SiteSettingDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SharedKernel.Shared;
using WebUILayer.Areas.Admin.Services.Abstract;
using WebUILayer.Areas.Admin.Services.Concrete;
using WebUILayer.Models;

namespace WebUILayer.Controllers;

public class MaintenanceController : Controller
{
    private readonly ISiteSettingsApiService _siteSettingsApiService;
    private readonly IContactApiService _contactApiService;
    private readonly AdminSettings _adminSettings;

    public MaintenanceController(ISiteSettingsApiService siteSettingsApiService, IContactApiService contactApiService, IOptions<AdminSettings> adminSettings)
    {
        _siteSettingsApiService = siteSettingsApiService;
        _contactApiService = contactApiService;
        _adminSettings = adminSettings.Value;
    }

    [HttpGet("/maintenance")]
    public async Task<IActionResult> Index()
    {
        try
        {
            var settings = await _siteSettingsApiService.GetSiteSettingForEditAsync();
            if (settings == null || !settings.IsMaintenanceMode)
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
        catch (Exception)
        {
            // Veritabanı komple çökerse "Ayarları" okuyamaz. 
            // O zaman statik veriler göndererek sayfanın (Bakımdayız ekranının) patlamasını engelleriz.
            var fallbackModel = new MaintenanceViewModel
            {
                siteSettingDtos = new UpdateSiteSettingDto
                {
                    MaintenanceMessage = "Sistemde geçici bir sorun yaşanıyor, hemen ilgileniyoruz.",
                    SiteTitle = "Sistem Bakımı"
                },
                contactDtos = new UpdateContactDto
                {
                    Email = _adminSettings.NotificationEmail,
                    Phone = "-"
                }
            };
            return View(fallbackModel);
        }
    }
}
