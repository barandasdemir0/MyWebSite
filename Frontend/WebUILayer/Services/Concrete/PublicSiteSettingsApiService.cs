using DtoLayer.SiteSettingDtos;
using WebUILayer.Services.Abstract;

namespace WebUILayer.Services.Concrete;

public class PublicSiteSettingsApiService : PublicReadApiService<SiteSettingDto>, IPublicSiteSettingsApiService
{
    public PublicSiteSettingsApiService(HttpClient httpClient/*, string endpoint*/) : base(httpClient, "sitesettings")
    {
    }

    public async Task<SiteSettingDto> GetPublicSiteSettingsAsync()
    {
        var response = await _httpClient.GetAsync($"{_endpoint}/single");
        if (!response.IsSuccessStatusCode) return new SiteSettingDto();
        return await response.Content.ReadFromJsonAsync<SiteSettingDto>() ?? new SiteSettingDto();
    }
}
