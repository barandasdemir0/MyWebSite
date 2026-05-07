using DtoLayer.AboutDtos;
using WebUILayer.Services.Abstract;

namespace WebUILayer.Services.Concrete;

public class PublicAboutApiService : PublicReadApiService<AboutDto>, IPublicAboutApiService
{
    public PublicAboutApiService(HttpClient httpClient/*, string endpoint*/) : base(httpClient, "abouts")
    {
    }

    public async Task<AboutDto> GetPublicAboutsAsync()
    {
        var response = await _httpClient.GetAsync($"{_endpoint}/single");
        if (!response.IsSuccessStatusCode) return new AboutDto();
        return await response.Content.ReadFromJsonAsync<AboutDto>() ?? new AboutDto();
    }
}
