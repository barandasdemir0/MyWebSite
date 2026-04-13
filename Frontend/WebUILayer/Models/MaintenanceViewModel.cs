using DtoLayer.ContactDtos;
using DtoLayer.SiteSettingDtos;

namespace WebUILayer.Models;

public class MaintenanceViewModel
{
    public UpdateSiteSettingDto? siteSettingDtos { get; set; } 
    public UpdateContactDto? contactDtos { get; set; } 
}
