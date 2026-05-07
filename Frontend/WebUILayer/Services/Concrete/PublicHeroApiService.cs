using DtoLayer.HeroDtos;
using WebUILayer.Services.Abstract;

namespace WebUILayer.Services.Concrete;

public class PublicHeroApiService : PublicReadApiService<HeroDto>, IPublicHeroApiService
{
    public PublicHeroApiService(HttpClient httpClient/*, string endpoint*/) : base(httpClient, "heroes")
    {
    }

    public async Task<HeroDto> GetPublicHeroesAsync()
    {
        var response = await _httpClient.GetAsync($"{_endpoint}/single");
        if (!response.IsSuccessStatusCode) return new HeroDto();
        return await response.Content.ReadFromJsonAsync<HeroDto>() ?? new HeroDto();
    }
}
