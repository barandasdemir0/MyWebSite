using DtoLayer.SiteSettingDtos;
using Mapster;
using WebUILayer.Areas.Admin.Services.Abstract;

namespace WebUILayer.Areas.Admin.Services.Concrete;

public class SiteSettingsApiService : GenericApiService<SiteSettingDto, CreateSiteSettingDto, UpdateSiteSettingDto>, ISiteSettingsApiService
{
    public SiteSettingsApiService(HttpClient httpClient/*, string endpoint*/) : base(httpClient, "sitesettings")
    {
    }

    public async Task<UpdateSiteSettingDto> GetSiteSettingForEditAsync()
    {
        var response = await _httpClient.GetAsync($"{_endpoint}/single");
        if (!response.IsSuccessStatusCode)
        {
            return new UpdateSiteSettingDto();
        }
        var query = await response.Content.ReadFromJsonAsync<SiteSettingDto>();
        if (query == null)
        {
            return new UpdateSiteSettingDto();
        }
        return query.Adapt<UpdateSiteSettingDto>();
    }


    //upsert tarafıdır
    public async Task SaveAboutAsync(UpdateSiteSettingDto updateSiteSettingDto)
    {
        var response = await _httpClient.PostAsJsonAsync($"{_endpoint}/save", updateSiteSettingDto);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception(error);
        }
    }
}
