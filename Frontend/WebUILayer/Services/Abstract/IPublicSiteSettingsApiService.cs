using DtoLayer.SiteSettingDtos;

namespace WebUILayer.Services.Abstract;

public interface IPublicSiteSettingsApiService:IPublicReadApiService<SiteSettingDto>
{
    Task<SiteSettingDto> GetPublicSiteSettingsAsync();
}
