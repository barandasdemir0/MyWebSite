using DtoLayer.SiteSettingDtos;

namespace WebUILayer.Areas.Admin.Services.Abstract;

public interface ISiteSettingsApiService:IGenericApiService<SiteSettingDto,CreateSiteSettingDto,UpdateSiteSettingDto>
{
    Task<UpdateSiteSettingDto> GetSiteSettingForEditAsync();

    Task SaveSiteSettingAsync(UpdateSiteSettingDto updateSiteSettingDto);
}
