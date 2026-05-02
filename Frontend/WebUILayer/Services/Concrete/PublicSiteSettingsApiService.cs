using DtoLayer.SiteSettingDtos;
using WebUILayer.Services.Abstract;

namespace WebUILayer.Services.Concrete;

public class PublicSiteSettingsApiService : PublicReadApiService<SiteSettingDto>, IPublicSiteSettingsApiService
{
    public PublicSiteSettingsApiService(HttpClient httpClient/*, string endpoint*/) : base(httpClient, "sitesettings")
    {
    }
}
