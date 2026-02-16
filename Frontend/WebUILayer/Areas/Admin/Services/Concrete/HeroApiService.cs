using DtoLayer.HeroDtos;
using Mapster;
using WebUILayer.Areas.Admin.Services.Abstract;

namespace WebUILayer.Areas.Admin.Services.Concrete
{
    public class HeroApiService : GenericApiService<HeroDto, CreateHeroDto, UpdateHeroDto>, IHeroApiService
    {
        public HeroApiService(HttpClient httpClient) : base(httpClient, "heroes")
        {
        }

        public async Task<UpdateHeroDto> GetHeroForEditAsync()
        {
            var response = await _httpClient.GetAsync($"{_endpoint}/single");
            if (!response.IsSuccessStatusCode)
            {
                return new UpdateHeroDto();
            }
            var query = await response.Content.ReadFromJsonAsync<HeroDto>();
            if (query==null)
            {
                return new UpdateHeroDto();
            }
            return query.Adapt<UpdateHeroDto>();
        }

        public async Task SaveHeroAsync(UpdateHeroDto updateHeroDto)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_endpoint}/save", updateHeroDto);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }
        }
    }
}
